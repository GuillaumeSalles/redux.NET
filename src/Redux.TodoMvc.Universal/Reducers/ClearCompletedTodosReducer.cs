using Redux.TodoMvc.Universal.Signals;
using Redux.TodoMvc.Universal.States;
using System.Collections.Immutable;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class ClearCompletedTodosReducer : Reducer<ImmutableArray<Todo>, ClearCompletedTodosSignal>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, ClearCompletedTodosSignal signal)
        {
            return previousState
                .RemoveAll(todo => todo.IsCompleted);
        }
    }
}
