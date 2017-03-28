using System;
using System.Reactive.Subjects;

namespace Redux
{
    public class Store<TState> : IStore<TState>
    {
        private readonly object _syncRoot = new object();
        private readonly Dispatcher _dispatcher;
        private readonly Reducer<TState> _reducer;
        private readonly ReplaySubject<TState> _stateSubject = new ReplaySubject<TState>(1);
        private TState _lastState;

        public Store(Reducer<TState> reducer, TState initialState = default(TState), params Middleware<TState>[] middlewares)
        {
            _reducer = reducer;
            _dispatcher = ApplyMiddlewares(middlewares);

            _lastState = initialState;
            _stateSubject.OnNext(_lastState);
        }

        public object Dispatch(object action)
        {
            return _dispatcher(action);
        }

        public TState GetState()
        {
            return _lastState;
        }
        
        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return _stateSubject
                .Subscribe(observer);
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
            _stateSubject.OnNext(_lastState);
            return action;
        }
    }
}
