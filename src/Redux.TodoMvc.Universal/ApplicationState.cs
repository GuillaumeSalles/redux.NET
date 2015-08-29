using System;
using System.Collections.Immutable;

namespace Redux.TodoMvc.Universal
{
    public class Todo
    {
        public string Text { get; set; }

        public bool IsCompleted { get; set; }

        public Guid Id { get; set; }
    }

    public enum TodosFilter
    {
        All,
        InProgress,
        Completed
    }

    public class ApplicationState
    {
        public ImmutableArray<Todo> Todos { get; set; }

        public TodosFilter Filter { get; set; }
    }
}
