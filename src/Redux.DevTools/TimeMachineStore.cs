namespace Redux.DevTools
{
    /// Todo : Refactor to use StoreEnhancer
    public class TimeMachineStore<TState> : Store<TimeMachineState>, IStore<TState>
    {
        public TimeMachineStore(Reducer<TState> reducer, TState initialState = default(TState))
            : base(new TimeMachineReducer((state, action) => reducer((TState)state, action)).Execute, new TimeMachineState(initialState))
        {
        }

        TState IStore<TState>.GetState()
        {
            return Unlift(GetState());
        }

        private TState Unlift(TimeMachineState state)
        {
            return (TState)state.States[state.Position];
        }
    }
}