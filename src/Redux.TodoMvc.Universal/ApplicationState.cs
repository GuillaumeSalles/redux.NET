using System;
using System.Collections.Immutable;

namespace Redux.TodoMvc.Universal
{
    public class Todo
    {
        public string Text { get; set; }

        public bool IsMarked { get; set; }

        public Guid Id { get; set; }
    }

    public enum TodosFilter
    {
        All,
        InProgress,
        Marked
    }

    public class ApplicationState
    {
        public ImmutableArray<Todo> Todos { get; set; }

        public TodosFilter Filter { get; set; }
    }
}
