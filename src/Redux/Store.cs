using System;
using System.Reactive.Subjects;

namespace Redux
{
    public delegate IAction Dispatcher(IAction action);

    public delegate TState Reducer<TState>(TState previousState, IAction action);

    public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> store);

    public interface IStore<TState> : IObservable<TState>
    {
        IAction Dispatch(IAction action);

        TState GetState();
    }
        
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

        public IAction Dispatch(IAction action)
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

        private IAction InnerDispatch(IAction action)
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
