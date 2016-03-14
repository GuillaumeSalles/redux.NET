using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools
{
    public static class Reducers
    {
        public static Reducer<DevToolsState> LiftReducer<TState>(Reducer<TState> reducer)
        {
            return (previousState, action) =>
            {
                if (action is ResumeTimeMachineAction)
                {
                    return previousState
                        .WithIsPaused(false)
                        .WithStates(previousState.States.Take(previousState.Position + 1).ToImmutableList())
                        .WithActions(previousState.Actions.Take(previousState.Position).ToImmutableList());
                }

                if (action is PauseTimeMachineAction)
                {
                    return previousState
                        .WithIsPaused(true);
                }

                if (action is SetTimeMachinePositionAction)
                {
                    return previousState
                        .WithPosition(((SetTimeMachinePositionAction)action).Position)
                        .WithIsPaused(true);
                }

                if (previousState.IsPaused)
                {
                    return previousState;
                }

                var innerState = reducer((TState)(previousState.States.Last()), action);

                return previousState
                    .WithStates(previousState.States.Add(innerState))
                    .WithActions(previousState.Actions.Add(action))
                    .WithPosition(previousState.Position + 1);
            };
        }
    }
}
