using System;

namespace Redux
{
    public delegate object Dispatcher(object action);

    public delegate TState Reducer<TState>(TState previousState, object action);

    public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> store);
}
