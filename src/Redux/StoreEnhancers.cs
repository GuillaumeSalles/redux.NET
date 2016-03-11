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

                var dispatcher = FunctionalHelpers.Compose(middlewares.Select(middleware => middleware(store)).ToArray())(store.Dispatch);
                
                return StoreFactory.CreateDummy(
                    store.GetState,
                    dispatcher,
                    store.Subscribe);
            };
        }
    }
}
