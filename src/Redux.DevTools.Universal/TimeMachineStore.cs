using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools.Universal
{
    public class TimeMachineStore<TState> : Store<TimeMachineState>, IStore<TState>
    {
        public TimeMachineStore(TState initialState, Func<TState, ISignal, TState> reducer)
            : base(new TimeMachineState(initialState), new TimeMachineReducer((state, signal) => reducer((TState)state, signal)).Execute)
        {
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return ((IObservable<TimeMachineState>)this)
                .Select(state => (TState)state.States[state.Position])
                .DistinctUntilChanged()
                .Subscribe(observer);
        }
    }
}
