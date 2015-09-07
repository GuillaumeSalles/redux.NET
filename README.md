# Redux.NET

Redux.NET is an attempt to bring [Redux](https://github.com/rackt/redux) concepts to .NET application development. (Only Windows 10 for now, but Android and IOS will come thanks to Xamarin)

### Motivation

After working on several MVVM applications (Silverlight, WPF, WinRT), I always feel uneasy when they start to get larger : 
* Two-way bindings lead to cascading updates, making it very difficult to predict what would change as the result of a single user interaction.
* Asynchronous operations make even more laborious to keep the flow of the application in mind.

[Dan Abramov](https://twitter.com/dan_abramov), a brilliant javascript developer, faced the same kind of [problems](http://rackt.github.io/redux/docs/introduction/Motivation.html) with complex web application. His solution? [Redux](https://github.com/rackt/redux)!

> The whole state of your app is stored in an object tree inside a single *Store*.
> The only way to change the state tree is to emit an *Action*, an object describing what happened.
> To specify how the actions transform the state tree, you write pure *Reducers*.

### Installation

You can grab the latest [Redux.NET Nuget package](https://www.nuget.org/packages/Redux.NET/) or from the NuGet package manager console :

    Install-Package Redux.NET

### Development Tools

The development tools contain a time machine debugger inspired by [Elm debugger](http://debug.elm-lang.org/) and [Redux DevTools](https://github.com/gaearon/redux-devtools).

You can get the dev tools package [via nuget](https://www.nuget.org/packages/Redux.NET.DevTools/) or via the Nuget package manager console : 

    Install-Package Redux.NET.DevTools

### Examples (Windows 10 only)

* Counter ([sources](https://github.com/GuillaumeSalles/redux.NET/tree/master/examples/counter))
* Todo app ([sources](https://github.com/GuillaumeSalles/redux.NET/tree/master/examples/todomvc))

![](http://i.imgur.com/3rgYjsL.gif)


### To do

* Documentation
* Examples for Android and IOS
* Real world scenario (Asynchronous, complex UI...)
