using System;

namespace Redux
{
    /// <summary>
    ///     Represents a store that encapsulates a state tree and is used to dispatch actions to update the
    ///     state tree.
    /// </summary>
    /// <typeparam name="TState">
    ///     The state tree type.
    /// </typeparam>
    public interface IStore<TState>
    {
        /// <summary>
        ///     Dispatches an action to the store.
        /// </summary>
        /// <param name="action">
        ///     The action to dispatch.
        /// </param>
        /// <returns>
        ///     Varies depending on store enhancers. With no enhancers Dispatch returns the action that 
        ///     was passed to it.
        /// </returns>
        object Dispatch(object action);

        /// <summary>
        ///     Gets the current state tree.
        /// </summary>
        /// <returns>
        ///     The current state tree.
        /// </returns>
        TState GetState();

        /// <summary>
        ///     Occurs after an action has been dispatched to the reducer.
        /// </summary>
        event Action StateChanged;

        /// <summary>
        ///     Occurs after an action has been dispatched to the reducer. The action is passed to the event
        ///     handlers.
        /// </summary>
        /// <remarks>
        ///     This event is primarily intended for extending the store. For subscribing to state changes, use <see cref="StateChanged" />.
        /// </remarks>
        event Action<object> ActionDispatched;
    }
}