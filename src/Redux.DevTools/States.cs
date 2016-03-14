using System.Collections.Immutable;

namespace Redux.DevTools
{
    public class DevToolsState
    {
        public ImmutableList<IAction> Actions { get; private set; }

        public ImmutableList<object> States { get; private set; }

        public int Position { get; private set; }

        public bool IsPaused { get; private set; }


        public DevToolsState()
        {
            Actions = ImmutableList<IAction>.Empty;
            States = ImmutableList<object>.Empty;
        }

        public DevToolsState(object initialState) : this()
        {
            States = States.Add(initialState);
        }

        public DevToolsState(DevToolsState other)
        {
            Actions = other.Actions;
            States = other.States;
            Position = other.Position;
            IsPaused = other.IsPaused;
        }

        public DevToolsState WithPosition(int position)
        {
            return new DevToolsState(this) { Position = position };
        }

        public DevToolsState WithIsPaused(bool isPaused)
        {
            return new DevToolsState(this) { IsPaused = isPaused };
        }

        public DevToolsState WithStates(ImmutableList<object> states)
        {
            return new DevToolsState(this) { States = states };
        }

        public DevToolsState WithActions(ImmutableList<IAction> actions)
        {
            return new DevToolsState(this) { Actions = actions };
        }
    }
}
