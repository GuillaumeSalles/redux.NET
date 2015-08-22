using Redux.TodoMvc.Universal.Signals;
using System.Collections.Immutable;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class ClearMarkedTodosReducer : Reducer<ImmutableArray<Todo>, ClearMarkedSignal>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, ClearMarkedSignal signal)
        {
            return previousState
                .RemoveAll(todo => todo.IsMarked);
        }
    }
}
