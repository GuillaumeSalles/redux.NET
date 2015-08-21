using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;

namespace Redux.DevTools.Universal
{
    public class TimeTravelState
    {
        public TimeTravelState()
        {
            Signals = ImmutableList<ISignal>.Empty;
            States = ImmutableList<object>.Empty;
        }

        public TimeTravelState(object initialState) : this()
        {
            States = States.Add(initialState);
        }

        public TimeTravelState(TimeTravelState other)
        {
            Signals = other.Signals;
            States = other.States;
            Position = other.Position;
        }

        public ImmutableList<ISignal> Signals { get; set; }

        public ImmutableList<object> States { get; set; }

        public int Position { get; set; }

        public TimeTravelState WithPosition(int position)
        {
            return new TimeTravelState(this) { Position = position };
        }
    }
        
    public class TimeTravelStateReducer : IReducer<TimeTravelState>
    {
        private IReducer<TimeTravelState>[] _timeMachineReducers = new[]
        {
            new SetTimeMachinePositionReducer()
        };

        private Func<object, ISignal, object> _reducer;

        public TimeTravelStateReducer(Func<object, ISignal, object> reducer)
        {
            _reducer = reducer;
        }

        public TimeTravelState Execute(TimeTravelState previousState, ISignal signal)
        {
            if(signal is ITimeMachineSignal)
            {
                return _timeMachineReducers.Combine()(previousState, signal);
            }

            var innerState = _reducer(previousState.States.Last(), signal);

            return new TimeTravelState
            {
                States = previousState.States.Add(innerState),
                Signals = previousState.Signals.Add(signal),
                Position = previousState.Position + 1
            };
        }
    }

    public interface ITimeMachineSignal : ISignal
    {

    }

    public class SetTimeMachinePositionSignal : ITimeMachineSignal
    {
        public int Position { get; set; }
    }

    public class SetTimeMachinePositionReducer : Reducer<TimeTravelState, SetTimeMachinePositionSignal>
    {
        protected override TimeTravelState Execute(TimeTravelState previousState, 
            SetTimeMachinePositionSignal signal)
        {
            return previousState.WithPosition(signal.Position);
        }
    }

    public class TimeTravellerStore<TState> : Store<TimeTravelState>, IStore<TState>
    {
        public TimeTravellerStore(TState initialState, Func<TState, ISignal, TState> reducer)
            : base(new TimeTravelState(initialState), new TimeTravelStateReducer((state,signal) => reducer((TState)state,signal)).Execute)
        {
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return ((IObservable<TimeTravelState>)this)
                .Select(state => (TState) state.States[state.Position])
                .DistinctUntilChanged()
                .Subscribe(observer);
        }
    }
}
