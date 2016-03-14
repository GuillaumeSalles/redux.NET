using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools
{
    public class Instruments
    {
        private class DevToolsStore<TState> : IStore<TState>
        {
            public IStore<DevToolsState> LiftedStore { get; private set; }

            public DevToolsStore(Reducer<TState> reducer, TState initialState)
            {
                LiftedStore = StoreFactory.Create(
                    Reducers.LiftReducer(reducer), 
                    new DevToolsState(initialState));
            }

            public IAction Dispatch(IAction action)
            {
                return LiftedStore.Dispatch(action);
            }

            public TState GetState()
            {
                var timeMachineState = LiftedStore.GetState();
                return (TState)(timeMachineState.States[timeMachineState.Position]);
            }

            public IDisposable Subscribe(IObserver<TState> observer)
            {
                return LiftedStore
                   .Select(state => (TState)(state.States[state.Position]))
                   .Subscribe(observer);
            }
        }

        public IStore<DevToolsState> LiftedStore { get; private set; }
        
        public StoreEnhancer<TState> Enhancer<TState>()
        {
            return storeCreator => (reducer, initialState) =>
            {
                var store = new DevToolsStore<TState>(reducer,initialState);

                LiftedStore = store.LiftedStore;

                return store;
            };
        }
    }
}
