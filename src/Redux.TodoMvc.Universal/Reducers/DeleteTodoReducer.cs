using System.Linq;
using System.Collections.Immutable;
using Redux.TodoMvc.Universal.Signals;
using Redux.TodoMvc.Universal.States;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class DeleteTodoReducer : Reducer<ImmutableArray<Todo>, DeleteTodoSignal>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, DeleteTodoSignal signal)
        {
            var todoToDelete = previousState.First(todo => todo.Id == signal.TodoId);

            return previousState
                .Remove(todoToDelete);
        }
    }
}
