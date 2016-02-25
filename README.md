# Redux.NET

Redux.NET is an attempt to bring [Redux](https://github.com/rackt/redux) concepts to .NET application development. Redux.NET is a PCL project, so it can be used with Xamarin (IOS, Android, Forms), Windows Universal applications, Windows 8.1 applications, WPF, etc...

[![Build status](https://img.shields.io/appveyor/ci/GuillaumeSalles/redux-net/master.svg?style=flat-square)](https://ci.appveyor.com/project/GuillaumeSalles/redux-net/branch/master)
[![Nuget](https://img.shields.io/nuget/v/Redux.NET.svg?style=flat-square)](https://www.nuget.org/packages/Redux.NET)

## Table of Contents

- [Motivation](#motivation)
- [Installation](#installation)
- [Quick start](#quick-start)
- [Performance Optimization](#performance-optimization)
- [Using DevTools](#using-devtools)
- [Examples](#examples)
- [License](#license)

## Motivation

After working on several MVVM applications (Silverlight, WPF, WinRT), I always feel uneasy when they start to get larger : 
* Two-way bindings lead to cascading updates, making it very difficult to predict what would change as the result of a single user interaction.
* Asynchronous operations make even more laborious to keep the flow of the application in mind.

[Dan Abramov](https://twitter.com/dan_abramov), a brilliant javascript developer, faced the same kind of [problems](http://rackt.github.io/redux/docs/introduction/Motivation.html) with complex web application. His solution? [Redux](https://github.com/rackt/redux)!

> The whole state of your app is stored in an object tree inside a single *Store*.
> The only way to change the state tree is to emit an *Action*, an object describing what happened.
> To specify how the actions transform the state tree, you write pure *Reducers*.

## Installation

You can grab the latest [Redux.NET Nuget package](https://www.nuget.org/packages/Redux.NET/) or from the NuGet package manager console :

    Install-Package Redux.NET

## Quick-start

#### Actions

*Actions* are payloads of information that send data from your application to your *store*. 
They only need to implement the markup interface Redux.IAction.

```C#
public class IncrementAction : IAction { }
    
public class DecrementAction : IAction { }
    
public class AddTodoAction : IAction
{
    public string Text { get; set; }
}
```

#### Reducers

A *reducer* is a [pure function](https://en.wikipedia.org/wiki/Pure_function) with ((TState)state, (IAction)action) => (TState)state signature.
It describes how an action transforms the state into the next state.

The shape of the state is up to you: it can be a primitive, an array or an object.
The only important part is that you should not mutate the state object, but return a new object if the state changes.

```C#
namespace Redux.Counter.Universal
{
    public static class CounterReducer
    {
        public static int Execute(int state, IAction action)
        {
            if(action is IncrementAction)
            {
                return state + 1;
            }

            if(action is DecrementAction)
            {
                return state - 1;
            }

            return state;
        }
    }
}
```

#### Store

The Store\<TState> is the class that bring *actions* and *reducer* together. The store has the following responsibilities:

* Holds application state of type TState.
* Allows state to be updated via Dispatch(IAction action).
* Registers listeners via Subscribe(IObserver<TState> observer). The Store\<TState> class implements IObservable<TState> so [ReactiveExtensions](https://github.com/Reactive-Extensions/Rx.NET) is a usefull tool to observe state changes.

It’s important to note that you’ll only have a single store in a Redux application. 
In the [examples](#examples), I keep it as a static property on the application class.

The Store constructor take an initial state, of type TState, and a reducer.

```C#
using Redux;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Redux.Counter.Universal
{
    public sealed partial class App : Application
    {
        public static IStore<int> CounterStore { get; private set; }

        public App()
        {
            InitializeComponent();
            
            CounterStore = new Store<int>(reducer:CounterReducer.Execute, initialState:0);
        }
    
        [...]
    }
}

```

The following code show how to subscribe to a store and to dispatch actions.

```C#
using Redux;
using System;
using Windows.UI.Xaml.Controls;
using System.Reactive.Linq;

namespace Redux.Counter.Universal
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            App.CounterStore.Subscribe(counter => CounterRun.Text = counter.ToString());
        }

        private void IncrementButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.CounterStore.Dispatch(new IncrementAction());
        }

        [...]
    }
}
```

## Performance Optimization

Let's take a closer look to a slightly modified version of the todos app example : 

```C#

    public sealed partial class MainSection : UserControl
    {
        public MainSection()
        {
            this.InitializeComponent();

            App.Store
                .Subscribe(state => TodosItemsControl.ItemsSource = GetFilteredTodos(state));
        }
        
        public static IEnumerable<Todo> GetFilteredTodos(ApplicationState state)
        {
            if (state.Filter == TodosFilter.Completed)
            {
                return state.Todos.Where(x => x.IsCompleted);
            }

            if (state.Filter == TodosFilter.InProgress)
            {
                return state.Todos.Where(x => !x.IsCompleted);
            }

            return state.Todos;
        }
    }

```

In this example, ```GetFilteredTodos``` is calculated every times the application state changed. If the state tree is large, or the calculation expensive, repeating the calculation on every update may cause performance problems. [ReactiveExtensions](https://github.com/Reactive-Extensions/Rx.NET) can help us to avoid unnecessary recalculations.

#### DistinctUntilChanged to the rescue

We would like to execute ```GetFilteredTodos``` only when the value ```state.Filter``` or ```state.Todos``` changes, but not when changes occur in other (unrelated) parts of the state tree. Rx provide an extension method on IObservable<T> [DistinctUntilChanged](https://msdn.microsoft.com/en-us/library/system.reactive.linq.observable.distinctuntilchanged(v=vs.103).aspx)  that surfaces values only if they are different from the previous value based on a key selector (or IEqualityComparer<T>).
We just need to call DistinctUntilChanged with the appropriate keySelector just before subscribing to the store. Anonymous Types are a handy tool to create selectors since two instances of the same anonymous type are equal only if all their properties are equal. (See Remarks section : [Anonymous Types](https://msdn.microsoft.com/en-us/library/bb397696.aspx))

To compute ```GetFilteredTodos``` only when necessary, we need to modify the previous code like this :

```C#
    App.Store
        .DistinctUntilChanged(state => new { state.Todos, state.Filter }) 
        .Subscribe(state => TodosItemsControl.ItemsSource = GetFilteredTodos(state));
```

## Using DevTools

![](http://i.imgur.com/3rgYjsL.gif)

The development tools contain a time machine debugger inspired by [Elm debugger](http://debug.elm-lang.org/) and [Redux DevTools](https://github.com/gaearon/redux-devtools).

You can get the dev tools package [via nuget](https://www.nuget.org/packages/Redux.NET.DevTools/) or via the Nuget package manager console : 

    Install-Package Redux.NET.DevTools
    
To use the time machine, just replace the Store with a **TimeMachineStore** and the application Frame with a **DevFrame**: 

```C#
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Redux.DevTools.Universal;

namespace Redux.Counter.Universal
{
    public sealed partial class App : Application
    {
        public static IStore<int> CounterStore { get; private set; }

        public App()
        {
            InitializeComponent();
            
            CounterStore = new TimeMachineStore<int>(CounterReducer.Execute, 0);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new DevFrame
                {
                    TimeMachineStore = (IStore<TimeMachineState>)CounterStore
                };
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }

            Window.Current.Activate();
        }
    }
}
```

## Examples 

* Counter ([sources](examples/counter))
  * Windows universal app : Redux.Counter.sln
* Todo app ([sources](examples/todomvc))
  * Windows universal app : Redux.TodoMvc.Universal.sln
  * Xamarin Andriod : Redux.TodoMvc.Android.sln  
  * Xamarin Forms (IOS + Android) : Redux.TodoMvc.Forms.sln
* Async http ([sources](examples/async))
  * Windows universal app : Redux.Async.sln

## License

MIT
