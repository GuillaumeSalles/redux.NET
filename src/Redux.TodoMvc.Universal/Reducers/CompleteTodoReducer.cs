using System.Linq;
using System.Collections.Immutable;
using Redux.TodoMvc.Universal.Signals;
using Redux.TodoMvc.Universal.States;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class CompleteTodoReducer : Reducer<ImmutableArray<Todo>, CompleteTodoSignal>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, CompleteTodoSignal signal)
        {
            var todoToEdit = previousState.First(todo => todo.Id == signal.TodoId);

            return previousState
                .Replace(todoToEdit, new Todo
                {
                    Id = todoToEdit.Id,
                    Text = todoToEdit.Text,
                    IsCompleted = !todoToEdit.IsCompleted
                });
        }
    }
}
