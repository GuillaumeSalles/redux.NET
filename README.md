# Redux.NET

Redux.NET is an attempt to bring [Redux](https://github.com/rackt/redux) concepts to .NET application development. (Only Windows 10 for now, but Android and IOS will come thanks to Xamarin)

### Motivation

After working on several MVVM applications (Silverlight, WPF, WinRT), I always feel uneasy when they start to get larger : 
* Two-way bindings lead to cascading updates, making it very difficult to predict what would change as the result of a single user interaction.
* Asynchronous operations make even more laborious to keep the flow of the application in mind.

[Dan Abramov](https://twitter.com/dan_abramov), a brilliant javascript developer, faced the same kind of [problems](http://rackt.github.io/redux/docs/introduction/Motivation.html) with complex web application. His solution? [Redux](https://github.com/rackt/redux)!

### Repository Content

* A [Store class](https://github.com/GuillaumeSalles/redux.net/blob/master/src/Redux/Store.cs) to hold the application state and push it to subscribers, thanks to [Rx](https://github.com/Reactive-Extensions/Rx.NET). [(Redux Store doc)](http://rackt.github.io/redux/docs/basics/Store.html)
* A time machine debugger inspired by [Elm debugger](http://debug.elm-lang.org/) and [Redux DevTools](https://github.com/gaearon/redux-devtools)
* A todo application for Windows 10 that use everything listed above !

![](http://i.imgur.com/3rgYjsL.gif)

### To do

* Documentation
* Reducers helpers
* Action creators
* TodoApp for Android and IOS
* Real world scenario (Asynchronous, complex UI...)
* Nuget
