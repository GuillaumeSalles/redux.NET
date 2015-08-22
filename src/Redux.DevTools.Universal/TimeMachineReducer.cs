using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools.Universal
{
    public class TimeMachineReducer : IReducer<TimeMachineState>
    {
        private IReducer<TimeMachineState>[] _timeMachineReducers = new[]
        {
            new SetTimeMachinePositionReducer()
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

            var innerState = _reducer(previousState.States.Last(), signal);

            return new TimeMachineState
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

    public class SetTimeMachinePositionReducer : Reducer<TimeMachineState, SetTimeMachinePositionSignal>
    {
        protected override TimeMachineState Execute(TimeMachineState previousState, 
            SetTimeMachinePositionSignal signal)
        {
            return previousState.WithPosition(signal.Position);
        }
    }    
}
