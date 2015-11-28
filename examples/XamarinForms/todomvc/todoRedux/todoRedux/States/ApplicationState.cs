using System;
using System.Collections.Immutable;

namespace todoRedux
{
    public class ApplicationState
    {
        public ImmutableArray<Todo> Todos { get; set; }

        public TodosFilter Filter { get; set; }
    }
}

