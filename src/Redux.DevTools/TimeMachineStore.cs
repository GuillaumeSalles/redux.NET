using System;
using System.Collections.Generic;

namespace Redux.DevTools
{
    /// Todo : Refactor to use StoreEnhancer
    public class TimeMachineStore<TState> : Store<TimeMachineState>, IStore<TState>
    {
        private Dictionary<Action<TState>, Action<TimeMachineState>> _handlersDictionary = new Dictionary<Action<TState>, Action<TimeMachineState>>();

        public TimeMachineStore(Reducer<TState> reducer, TState initialState = default(TState))
            : base(new TimeMachineReducer((state, action) => reducer((TState)state, action)).Execute, new TimeMachineState(initialState))
        {
        }

        event Action<TState> IStore<TState>.StateChanged
        {
            add
            {
                var liftedHandler = LiftHandler(value);
                _handlersDictionary.Add(value, liftedHandler);
                StateChanged += liftedHandler;
            }
            remove
            {
                StateChanged -= _handlersDictionary[value];
                _handlersDictionary.Remove(value);
            }
        }

        TState IStore<TState>.GetState()
        {
            return Unlift(GetState());
        }

        private Action<TimeMachineState> LiftHandler(Action<TState> handler)
        {
            return timeMachineState => handler(Unlift(timeMachineState));
        }

        private TState Unlift(TimeMachineState state)
        {
            return (TState)state.States[state.Position];
        }
    }
}