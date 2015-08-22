using Redux.TodoMvc.Universal.Signals;
using System.Collections.Immutable;
using System.Linq;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class MarkTodoReducer : Reducer<ImmutableArray<Todo>, MarkTodoSignal>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, MarkTodoSignal signal)
        {
            var todoToEdit = previousState.First(todo => todo.Id == signal.TodoId);

            return previousState
                .Replace(todoToEdit, new Todo
                {
                    Id = todoToEdit.Id,
                    Text = todoToEdit.Text,
                    IsMarked = !todoToEdit.IsMarked
                });
        }
    }
}
