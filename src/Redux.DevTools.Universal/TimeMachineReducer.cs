using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.DevTools.Universal
{
    public static class ReducerExtensions
    {
        public static Func<TState, IAction, TState> Combine<TState>(this IEnumerable<IReducer<TState>> reducers)
        {
            return (state, action) =>
            {
                foreach (var reducer in reducers)
                {
                    state = reducer.Execute(state, action);
                }

                return state;
            };
        }
    }

    public interface IReducer<TState>
    {
        TState Execute(TState previousState, IAction action);
    }

    public abstract class Reducer<TState, TAction> : IReducer<TState>
        where TAction : IAction
    {
        public TState Execute(TState previousState, IAction action)
        {
            if (action is TAction)
            {
                return Execute(previousState, (TAction)action);
            }

            return previousState;
        }

        protected abstract TState Execute(TState previousState, TAction action);
    }

    public class TimeMachineReducer : IReducer<TimeMachineState>
    {
        private IReducer<TimeMachineState>[] _timeMachineReducers = new IReducer<TimeMachineState>[]
        {
            new SetTimeMachinePositionReducer(),
            new PauseTimeMachineReducer(),
            new ResumeTimeMachineReducer()
        };

        private Func<object, IAction, object> _reducer;

        public TimeMachineReducer(Func<object, IAction, object> reducer)
        {
            _reducer = reducer;
        }

        public TimeMachineState Execute(TimeMachineState previousState, IAction action)
        {
            if(action is ITimeMachineAction)
            {
                return _timeMachineReducers.Combine()(previousState, action);
            }

            if (previousState.IsPaused)
            {
                return previousState;
            }

            var innerState = _reducer(previousState.States.Last(), action);

            return previousState
                .WithStates(previousState.States.Add(innerState))
                .WithActions(previousState.Actions.Add(action))
                .WithPosition(previousState.Position + 1);
        }
    }

    public interface ITimeMachineAction : IAction
    {

    }

    public class PauseTimeMachineAction : ITimeMachineAction
    {

    }

    public class ResumeTimeMachineAction : ITimeMachineAction
    {

    }

    public class SetTimeMachinePositionAction : ITimeMachineAction
    {
        public int Position { get; set; }
    }

    public class SetTimeMachinePositionReducer : Reducer<TimeMachineState, SetTimeMachinePositionAction>
    {
        protected override TimeMachineState Execute(TimeMachineState previousState, 
            SetTimeMachinePositionAction action)
        {
            return previousState
                .WithPosition(action.Position)
                .WithIsPaused(true);
        }
    }

    public class ResumeTimeMachineReducer : Reducer<TimeMachineState, ResumeTimeMachineAction>
    {
        protected override TimeMachineState Execute(TimeMachineState previousState, 
            ResumeTimeMachineAction action)
        {
            return previousState
                .WithIsPaused(false)
                .WithStates(previousState.States.Take(previousState.Position + 1).ToImmutableList())
                .WithActions(previousState.Actions.Take(previousState.Position).ToImmutableList());
        }
    }

    public class PauseTimeMachineReducer : Reducer<TimeMachineState, PauseTimeMachineAction>
    {
        protected override TimeMachineState Execute(TimeMachineState previousState, 
            PauseTimeMachineAction action)
        {
            return previousState
                .WithIsPaused(true);
        }
    }
}
