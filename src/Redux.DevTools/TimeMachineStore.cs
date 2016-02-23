using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Redux.DevTools
{
    public class TimeMachineStore<TState> : Store<TimeMachineState>, IStore<TState>
    {
        public TimeMachineStore(TState initialState, Reducer<TState> reducer)
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

        TState IStore<TState>.GetState()
        {
            return ((IObservable<TState>)this)
                .FirstAsync().ToTask().Result;
        }
    }
}
