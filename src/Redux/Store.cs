using System;
using System.Collections.Generic;

namespace Redux
{
    public class Store<TState> : IStore<TState>
    {
        private readonly List<Action> _listeners = new List<Action>(); 
        private readonly object _syncRoot = new object();
        private readonly Dispatcher _dispatcher;
        private readonly Reducer<TState> _reducer;
        private TState _lastState;

        public Store(Reducer<TState> reducer, TState initialState = default(TState), params Middleware<TState>[] middlewares)
        {
            _reducer = reducer;
            _dispatcher = ApplyMiddlewares(middlewares);

            _lastState = initialState;
        }

        public object Dispatch(object action)
        {
            return _dispatcher(action);
        }

        public TState GetState()
        {
            return _lastState;
        }

        public Action Subscribe(Action listener)
        {
            _listeners.Add(listener);
            //Todo : Add tests and Remove to behave like redux.js
            listener();
            return () => _listeners.Remove(listener);
        }

        private Dispatcher ApplyMiddlewares(params Middleware<TState>[] middlewares)
        {
            Dispatcher dispatcher = InnerDispatch;
            foreach (var middleware in middlewares)
            {
                dispatcher = middleware(this)(dispatcher);
            }
            return dispatcher;
        }

        private object InnerDispatch(object action)
        {
            lock (_syncRoot)
            {
                _lastState = _reducer(_lastState, action);
            }

            foreach(var listener in _listeners)
            {
                listener();
            }

            return action;
        }
    }
}