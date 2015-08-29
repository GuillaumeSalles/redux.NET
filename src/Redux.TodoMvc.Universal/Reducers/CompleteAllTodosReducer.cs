using System.Collections.Immutable;
using System.Linq;
using Redux.TodoMvc.Universal.Signals;
using Redux.TodoMvc.Universal.States;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class CompleteAllTodosReducer : Reducer<ImmutableArray<Todo>, CompleteAllTodosSignal>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, CompleteAllTodosSignal signal)
        {
            return previousState
                .Select(x => new Todo
                {
                    Id = x.Id,
                    Text = x.Text,
                    IsCompleted = signal.IsCompleted
                })
                .ToImmutableArray();
        }
    }
}
