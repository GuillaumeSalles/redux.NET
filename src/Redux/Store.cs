using System;

namespace Redux
{
    public interface IStore<TState>
    {
        object Dispatch(object action);

        TState GetState();

        event Action<TState> StateChanged;
    }

    public class Store<TState> : IStore<TState>
    {
        private readonly object _syncRoot = new object();
        private readonly Dispatcher _dispatcher;
        private Action<TState> _stateChanged;
        private readonly Reducer<TState> _reducer;
        private TState _lastState;

        public Store(Reducer<TState> reducer, TState initialState = default(TState), params Middleware<TState>[] middlewares)
        {
            _reducer = reducer;
            _dispatcher = ApplyMiddlewares(middlewares);

            _lastState = initialState;
        }

        public event Action<TState> StateChanged
        {
            add
            {
                _stateChanged += value;
                value(_lastState);
            }
            remove
            {
                _stateChanged -= value;
            }
        }

        public object Dispatch(object action)
        {
            return _dispatcher(action);
        }

        public TState GetState()
        {
            return _lastState;
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
            _stateChanged?.Invoke(_lastState);
            return action;
        }
    }
}
