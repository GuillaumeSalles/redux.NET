using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools
{

    public class TimeMachineStore<TState> : Store<TimeMachineState>, IStore<TState>
    {
        public static Middleware<TimeMachineState> TimeMachineMiddleware(Middleware<TState> middleware)
        {
            return store => next => action => middleware((IStore<TState>)store)(next)(action);
        }

        public TimeMachineStore(
            Reducer<TState> reducer, 
            TState initialState = default(TState), 
            params Middleware<TState>[] middlewares
        ) : base(
            new TimeMachineReducer((state, action) => reducer((TState)state, action)).Execute, 
            new TimeMachineState(initialState), 
            middlewares.Select(m => TimeMachineMiddleware(m)).ToArray()
        )
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
            int position = this.GetState().Position;
            return (TState)this.GetState().States[position];
        }
    }
}
