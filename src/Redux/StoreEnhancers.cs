using System.Linq;
using static Redux.FunctionalHelpers;

namespace Redux
{
    public static class StoreEnhancers
    {
        public static StoreEnhancer<TState> ApplyMiddleware<TState>(params Middleware<TState>[] middlewares)
        {
            return (storeCreator) => (reducer,initialState) => 
            {
                var store = storeCreator(reducer, initialState);

                var dispatcher = Compose(middlewares.Select(middleware => middleware(store)).ToArray())(store.Dispatch);
                
                return StoreFactory.CreateDummy(
                    store.GetState,
                    dispatcher,
                    store.Subscribe);
            };
        }
    }
}
