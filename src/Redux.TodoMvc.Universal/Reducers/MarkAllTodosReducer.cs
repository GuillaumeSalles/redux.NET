using Redux.TodoMvc.Universal.Signals;
using System.Collections.Immutable;
using System.Linq;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class MarkAllTodosReducer : Reducer<ImmutableArray<Todo>, MarkAllTodos>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, MarkAllTodos signal)
        {
            return previousState
                .Select(x => new Todo
                {
                    Id = x.Id,
                    Text = x.Text,
                    IsMarked = signal.IsMarked
                })
                .ToImmutableArray();
        }
    }
}
