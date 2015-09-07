# Redux.NET

Redux.NET is an attempt to bring [Redux](https://github.com/rackt/redux) concepts to .NET application development. (Only Windows 10 for now, but Android and IOS will come thanks to Xamarin)

## Table of Contents

- [Motivation](#motivation)
- [Installation](#installation)
- [Quick start](#quick-start)
- [Using DevTools](#using-devtools)
- [Examples](#examples)


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

Actions are payloads of information that send data from your application to your store. 
They only need to implement the markup interface Redux.IAction.

```
    public class IncrementAction : IAction { }
    
    public class DecrementAction : IAction { }
    
    public class AddTodoAction : IAction
    {
        public string Text { get; set; }
    }
```

#### Reducers

A reducer is a pure function with ((TState)state, (IAction)action) => (TState)state signature.
It describes how an action transforms the state into the next state.

The shape of the state is up to you: it can be a primitive, an array or an object.
The only important part is that you should not mutate the state object, but return a new object if the state changes.

```
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
```

#### Store

The Store\<TState> is the class that bring *actions* and *reducer* together. The store has the following responsibilities:

* Holds application state of type TState.
* Allows state to be updated via Dispatch(IAction action).
* Registers listeners via Subscribe(IObserver<TState> observer). The Store\<TState> class implements IObservable<TState> so [ReactiveExtensions](https://github.com/Reactive-Extensions/Rx.NET) is a usefull tool to observe state changes.

It’s important to note that you’ll only have a single store in a Redux application. 
In the [examples](#examples), I keep it as a static property on the application class.

The Store constructor take an initial state, of type TState, and a reducer.

```
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
            
            CounterStore = new Store<int>(initialState:0, reducer:CounterReducer.Execute);
        }
    
        [...]
    }
}

```

The following code show how to subscribe to a store and to dispatch actions.

```
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

## Using DevTools

The development tools contain a time machine debugger inspired by [Elm debugger](http://debug.elm-lang.org/) and [Redux DevTools](https://github.com/gaearon/redux-devtools).

You can get the dev tools package [via nuget](https://www.nuget.org/packages/Redux.NET.DevTools/) or via the Nuget package manager console : 

    Install-Package Redux.NET.DevTools
    
Todo : Documentation

## Examples 

* Counter ([sources](https://github.com/GuillaumeSalles/redux.NET/tree/master/examples/counter))
* Todo app ([sources](https://github.com/GuillaumeSalles/redux.NET/tree/master/examples/todomvc))

![](http://i.imgur.com/3rgYjsL.gif)


## To do

* Documentation
* Examples for Android and IOS
* Real world scenario (Asynchronous, complex UI...)
