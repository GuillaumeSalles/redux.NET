using System;
using System.Linq;

namespace Redux
{
    public static class StoreEnhancers
    {
        public static StoreEnhancer<TState> Compose<TState>(params StoreEnhancer<TState>[] enhancers)
        {
            return arg =>
            {
                for (int i = enhancers.Length - 1; i >= 0; i--)
                {
                    arg = enhancers[i](arg);
                };

                return arg;
            };
        }

        public static StoreEnhancer<TState> ApplyMiddleware<TState>(params Middleware<TState>[] middlewares)
        {
            return storeCreator => (reducer,initialState) => 
            {
                var store = storeCreator(reducer, initialState);

                return new ApplyMiddlewareStore<TState>(store, middlewares);
            };
        }
        
        private class ApplyMiddlewareStore<TState> : IStore<TState>
        {
            private readonly IStore<TState> _innerStore;
            private readonly Dispatcher _dispatcher;

            public ApplyMiddlewareStore(IStore<TState> innerStore, params Middleware<TState>[] middlewares)
            {
                _innerStore = innerStore;
                _dispatcher = FunctionalHelpers.Compose(middlewares.Select(middleware => middleware(innerStore)).ToArray())(innerStore.Dispatch);
            }

            public IAction Dispatch(IAction action)
            {
                return _dispatcher(action);
            }

            public TState GetState()
            {
                return _innerStore.GetState();
            }

            public IDisposable Subscribe(IObserver<TState> observer)
            {
                return _innerStore.Subscribe(observer);
            }
        }
    }
}
