using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools
{
    public class Instruments
    {
        public IStore<TimeMachineState> LiftedStore { get; private set; }

        private Reducer<TimeMachineState> LiftReducer<TState>(Reducer<TState> reducer)
        {
            return new TimeMachineReducer((state, action) => reducer((TState)state, action)).Execute;
        }

        private Func<TState> UnliftGetState<TState>(IStore<TimeMachineState> liftedStore)
        {
            return () =>
            {
                var timeMachineState = liftedStore.GetState();
                return (TState)(timeMachineState.States[timeMachineState.Position]);
            };
        }

        private Func<IObserver<TState>, IDisposable> UnliftSubscribe<TState>(
            IStore<TimeMachineState> liftedStore)
        {
            return liftedStore
                .Select(state => (TState)(state.States[state.Position]))
                .Subscribe;
        }

        public StoreEnhancer<TState> Enhancer<TState>()
        {
            return storeCreator => (reducer, initialState) =>
            {
                LiftedStore = StoreFactory.Create(LiftReducer(reducer),new TimeMachineState(initialState));

                return StoreFactory.CreateDummy(
                    UnliftGetState<TState>(LiftedStore),
                    LiftedStore.Dispatch,
                    UnliftSubscribe<TState>(LiftedStore));
            };
        }
    }
}
