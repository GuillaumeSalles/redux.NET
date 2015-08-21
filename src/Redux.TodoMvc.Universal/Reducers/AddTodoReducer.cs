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

    public class ClearMarkedReducer : Reducer<ImmutableArray<Todo>, ClearMarkedSignal>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, ClearMarkedSignal signal)
        {
            return previousState
                .RemoveAll(todo => todo.IsMarked);
        }
    }

    public class FilterTodosReducer : Reducer<TodosFilter, FilterTodosSignal>
    {
        protected override TodosFilter Execute(TodosFilter previousState, FilterTodosSignal signal)
        {
            return signal.Filter;
        }
    }

    public class MarkAllReducer : Reducer<ImmutableArray<Todo>, MarkAllSignal>
    {
        protected override ImmutableArray<Todo> Execute(
            ImmutableArray<Todo> previousState, MarkAllSignal signal)
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
