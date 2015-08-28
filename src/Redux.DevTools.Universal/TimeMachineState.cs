using System.Collections.Immutable;

namespace Redux.DevTools.Universal
{
    public class TimeMachineState
    {
        public ImmutableList<ISignal> Signals { get; private set; }

        public ImmutableList<object> States { get; private set; }

        public int Position { get; private set; }

        public bool IsPaused { get; private set; }


        public TimeMachineState()
        {
            Signals = ImmutableList<ISignal>.Empty;
            States = ImmutableList<object>.Empty;
        }

        public TimeMachineState(object initialState) : this()
        {
            States = States.Add(initialState);
        }

        public TimeMachineState(TimeMachineState other)
        {
            Signals = other.Signals;
            States = other.States;
            Position = other.Position;
            IsPaused = other.IsPaused;
        }

        public TimeMachineState WithPosition(int position)
        {
            return new TimeMachineState(this) { Position = position };
        }

        public TimeMachineState WithIsPaused(bool isPaused)
        {
            return new TimeMachineState(this) { IsPaused = isPaused };
        }

        public TimeMachineState WithStates(ImmutableList<object> states)
        {
            return new TimeMachineState(this) { States = states };
        }

        public TimeMachineState WithSignals(ImmutableList<ISignal> signals)
        {
            return new TimeMachineState(this) { Signals = signals };
        }
    }
}
