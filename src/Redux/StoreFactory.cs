namespace Redux
{
    public static class StoreFactory
    {
        public static IStore<TState> Create<TState>(
            Reducer<TState> reducer,
            TState initialState = default(TState))
        {
            return new Store<TState>(reducer, initialState);
        }

        public static IStore<TState> Create<TState>(
            Reducer<TState> reducer,
            StoreEnhancer<TState> storeEnhancer = null)
        {
            return Create(reducer, default(TState), storeEnhancer);
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
