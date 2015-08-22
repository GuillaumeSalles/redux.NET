using System.Collections.Immutable;

namespace Redux.DevTools.Universal
{
    public class TimeMachineState
    {
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
        }

        public ImmutableList<ISignal> Signals { get; set; }

        public ImmutableList<object> States { get; set; }

        public int Position { get; set; }

        public TimeMachineState WithPosition(int position)
        {
            return new TimeMachineState(this) { Position = position };
        }
    }
}
