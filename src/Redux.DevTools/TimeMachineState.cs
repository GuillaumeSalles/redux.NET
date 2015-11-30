using System.Collections.Immutable;

namespace Redux.DevTools
{
    public class TimeMachineState
    {
        public ImmutableList<IAction> Actions { get; private set; }

        public ImmutableList<object> States { get; private set; }

        public int Position { get; private set; }

        public bool IsPaused { get; private set; }


        public TimeMachineState()
        {
            Actions = ImmutableList<IAction>.Empty;
            States = ImmutableList<object>.Empty;
        }

        public TimeMachineState(object initialState) : this()
        {
            States = States.Add(initialState);
        }

        public TimeMachineState(TimeMachineState other)
        {
            Actions = other.Actions;
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

        public TimeMachineState WithActions(ImmutableList<IAction> actions)
        {
            return new TimeMachineState(this) { Actions = actions };
        }
    }
}
