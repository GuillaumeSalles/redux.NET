using System;

namespace Redux 
{
    /// <summary>
    ///     Represents a method that dispatches an action.
    /// </summary>
    /// <param name="action">
    ///     The action to dispatch.
    /// </param>
    /// <returns>
    ///     Varies
    /// </returns>
    public delegate object Dispatcher(object action);

    /// <summary>
    ///     Represents a method that is used as middleware.
    /// </summary>
    /// <typeparam name="TState">
    ///     The state tree type.
    /// </typeparam>
    /// <param name="store">
    ///     The <see cref="IStore{TState}" /> this middleware is to be used by.
    /// </param>
    /// <returns>
    ///     A function that, when called with a <see cref="Dispatcher" />, returns a new
    ///     <see cref="Dispatcher" /> that wraps the first one.
    /// </returns>
    public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> store);

    /// <summary>
    ///     Represents a method that is used to update the state tree.
    /// </summary>
    /// <typeparam name="TState">
    ///     The state tree type.
    /// </typeparam>
    /// <param name="previousState">
    ///     The previous state tree.
    /// </param>
    /// <param name="action">
    ///     The action to be applied to the state tree.
    /// </param>
    /// <returns>
    ///     The updated state tree.
    /// </returns>
    public delegate TState Reducer<TState>(TState previousState, object action);
}