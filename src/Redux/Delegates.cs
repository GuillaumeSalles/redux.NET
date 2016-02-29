using System;

namespace Redux
{
    public delegate IAction Dispatcher(IAction action);

    public delegate TState Reducer<TState>(TState previousState, IAction action);

    public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> store);

    public delegate IStore<TState> StoreCreator<TState>(Reducer<TState> reducer, TState initialState);

    public delegate StoreCreator<TState> StoreEnhancer<TState>(StoreCreator<TState> storeCreator);
}
