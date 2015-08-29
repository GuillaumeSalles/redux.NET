# Redux.net

Redux.net is an attempt to bring [Redux](https://github.com/rackt/redux) concepts to .NET application development. (Only Windows 10 for now, but Android, IOS will come thanks to Xamarin)

### Repository Content

* A [Store class](https://github.com/GuillaumeSalles/redux.net/blob/master/src/Redux/Store.cs) to keep your application state and push state modifications to your view.  [(Redux Store doc)](http://rackt.github.io/redux/docs/basics/Store.html)
* [Helpers](https://github.com/GuillaumeSalles/redux.net/blob/master/src/Redux/Reducer.cs) to create reducers. [(Redux Reducer doc)](http://rackt.github.io/redux/docs/basics/Reducers.html)
* A time machine debugger inspired by [Elm debugger](http://debug.elm-lang.org/) and [Redux DevTools](https://github.com/gaearon/redux-devtools)
* A todo application for Windows 10 that use everything listed above !

![](http://i.imgur.com/RmpugpV.gif)

### To do

* Documentation
* TodoApp for Android and IOS
* Real world scenario (Asynchronous, complex UI...)
* Nuget
