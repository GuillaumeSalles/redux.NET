using System;
using System.Collections.Generic;

namespace Redux
{
    public static class ReducerExtensions
    {
        public static Func<TState,ISignal,TState> Combine<TState>(this IEnumerable<IReducer<TState>> reducers)
        {
            return (state, signal) =>
            {
                foreach(var reducer in reducers)
                {
                    state = reducer.Execute(state, signal);
                }

                return state;
            };
        }
    }

    public interface IReducer<TState>
    {
        TState Execute(TState previousState, ISignal signal);
    }

    public abstract class Reducer<TState, TSignal> : IReducer<TState>
        where TSignal : ISignal
    {
        public TState Execute(TState previousState, ISignal signal)
        {
            if(signal is TSignal)
            {
                return Execute(previousState, (TSignal)signal);
            }

            return previousState;
        }

        protected abstract TState Execute(TState previousState, TSignal signal);
    }
}
