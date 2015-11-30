using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools
{
    public class TimeMachineStore<TState> : Store<TimeMachineState>, IStore<TState>
    {
        public TimeMachineStore(TState initialState, Func<TState, IAction, TState> reducer)
            : base(new TimeMachineState(initialState), new TimeMachineReducer((state, action) => reducer((TState)state, action)).Execute)
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
