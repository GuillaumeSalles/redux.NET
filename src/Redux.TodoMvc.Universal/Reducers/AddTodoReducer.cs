using System;
using System.Collections.Immutable;
using System.Linq;
using Redux.TodoMvc.Universal.Signals;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class AddTodoReducer : Reducer<ImmutableArray<Todo>, AddTodoSignal>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, AddTodoSignal signal)
        {
            return previousState
                .Insert(0,new Todo
                {
                    Id = Guid.NewGuid(),
                    Text = signal.Text
                });  
        }
    }
}
