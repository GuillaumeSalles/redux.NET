using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools.Universal
{
    public class TimeMachineReducer : IReducer<TimeMachineState>
    {
        private IReducer<TimeMachineState>[] _timeMachineReducers = new IReducer<TimeMachineState>[]
        {
            new SetTimeMachinePositionReducer(),
            new PauseTimeMachineReducer(),
            new ResumeTimeMachineReducer()
        };

        private Func<object, ISignal, object> _reducer;

        public TimeMachineReducer(Func<object, ISignal, object> reducer)
        {
            _reducer = reducer;
        }

        public TimeMachineState Execute(TimeMachineState previousState, ISignal signal)
        {
            if(signal is ITimeMachineSignal)
            {
                return _timeMachineReducers.Combine()(previousState, signal);
            }

            if (previousState.IsPaused)
            {
                return previousState;
            }

            var innerState = _reducer(previousState.States.Last(), signal);

            return previousState
                .WithStates(previousState.States.Add(innerState))
                .WithSignals(previousState.Signals.Add(signal))
                .WithPosition(previousState.Position + 1);
        }
    }

    public interface ITimeMachineSignal : ISignal
    {

    }

    public class PauseTimeMachineSignal : ITimeMachineSignal
    {

    }

    public class ResumeTimeMachineSignal : ITimeMachineSignal
    {

    }

    public class SetTimeMachinePositionSignal : ITimeMachineSignal
    {
        public int Position { get; set; }
    }

    public class SetTimeMachinePositionReducer : Reducer<TimeMachineState, SetTimeMachinePositionSignal>
    {
        protected override TimeMachineState Execute(TimeMachineState previousState, 
            SetTimeMachinePositionSignal signal)
        {
            return previousState
                .WithPosition(signal.Position)
                .WithIsPaused(true);
        }
    }

    public class ResumeTimeMachineReducer : Reducer<TimeMachineState, ResumeTimeMachineSignal>
    {
        protected override TimeMachineState Execute(TimeMachineState previousState, 
            ResumeTimeMachineSignal signal)
        {
            return previousState
                .WithIsPaused(false)
                .WithStates(previousState.States.Take(previousState.Position + 1).ToImmutableList())
                .WithSignals(previousState.Signals.Take(previousState.Position).ToImmutableList());
        }
    }

    public class PauseTimeMachineReducer : Reducer<TimeMachineState, PauseTimeMachineSignal>
    {
        protected override TimeMachineState Execute(TimeMachineState previousState, 
            PauseTimeMachineSignal signal)
        {
            return previousState
                .WithIsPaused(true);
        }
    }
}
