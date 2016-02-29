using System;

namespace Redux
{
    public static class StoreFactory
    {
        private class DummyStore<TState> : IStore<TState>
        {
            Func<TState> _getState;
            Dispatcher _dispatcher;
            Func<IObserver<TState>, IDisposable> _subscribe;

            public DummyStore(
                Func<TState> getState,
                Dispatcher dispatcher,
                Func<IObserver<TState>, IDisposable> subscribe)
            {
                _getState = getState;
                _dispatcher = dispatcher;
                _subscribe = subscribe;
            }

            public IAction Dispatch(IAction action)
            {
                return _dispatcher(action);
            }

            public TState GetState()
            {
                return _getState();
            }

            public IDisposable Subscribe(IObserver<TState> observer)
            {
                return _subscribe(observer);
            }
        }

        public static IStore<TState> CreateDummy<TState>(
            Func<TState> getState,
            Dispatcher dispatcher,
            Func<IObserver<TState>, IDisposable> subscribe)
        {
            return new DummyStore<TState>(getState, dispatcher, subscribe);
        }

        public static IStore<TState> Create<TState>(
            Reducer<TState> reducer,
            TState initialState = default(TState))
        {
            return new Store<TState>(reducer, initialState);
        }

        public static IStore<TState> Create<TState>(
            Reducer<TState> reducer, 
            TState initialState = default(TState),
            StoreEnhancer<TState> storeEnhancer = null)
        {
            if(storeEnhancer == null)
            {
                return Create(reducer, initialState);
            }

            return storeEnhancer(Create)(reducer, initialState);
        }
    }
}
