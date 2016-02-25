using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools
{
    public class TimeMachineStore<TState> : Store<TimeMachineState>, IStore<TState>
    {
        public TimeMachineStore(Reducer<TState> reducer, TState initialState = default(TState))
            : base(new TimeMachineReducer((state, action) => reducer((TState)state, action)).Execute, new TimeMachineState(initialState))
        {
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return ((IObservable<TimeMachineState>)this)
                .Select(state => (TState)state.States[state.Position])
                .DistinctUntilChanged()
                .Subscribe(observer);
        }

        TState IStore<TState>.GetState()
        {
            return ((IStore<TState>)this).GetState();
        }
    }
}
