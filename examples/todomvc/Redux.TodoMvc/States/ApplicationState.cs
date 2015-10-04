using System.Collections.Immutable;

namespace Redux.TodoMvc.States
{
    public class ApplicationState
    {
        public ImmutableArray<Todo> Todos { get; set; }

        public TodosFilter Filter { get; set; }
    }
}
