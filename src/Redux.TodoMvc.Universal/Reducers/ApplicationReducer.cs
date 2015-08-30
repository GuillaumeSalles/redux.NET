using Redux.TodoMvc.Universal.Signals;
using Redux.TodoMvc.Universal.States;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Redux.TodoMvc.Universal.Reducers
{
    public static class ApplicationReducer
    {
        public static ImmutableArray<Todo> AddTodoReducer(ImmutableArray<Todo> previousState, AddTodoSignal signal)
        {
            return previousState
                .Insert(0, new Todo
                {
                    Id = Guid.NewGuid(),
                    Text = signal.Text
                });
        }

        public static ImmutableArray<Todo> ClearCompletedTodosReducer(ImmutableArray<Todo> previousState, ClearCompletedTodosSignal signal)
        {
            return previousState.RemoveAll(todo => todo.IsCompleted);
        }

        public static ImmutableArray<Todo> CompleteAllTodosReducer(ImmutableArray<Todo> previousState, CompleteAllTodosSignal signal)
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

        public static ImmutableArray<Todo> CompleteTodoReducer(ImmutableArray<Todo> previousState, CompleteTodoSignal signal)
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

        public static ImmutableArray<Todo> DeleteTodoReducer(ImmutableArray<Todo> previousState, DeleteTodoSignal signal)
        {
            var todoToDelete = previousState.First(todo => todo.Id == signal.TodoId);

            return previousState.Remove(todoToDelete);
        }

        public static ImmutableArray<Todo> TodosReducer(ImmutableArray<Todo> previousState, ISignal signal)
        {
            if (signal is AddTodoSignal)
            {
                return AddTodoReducer(previousState, (AddTodoSignal)signal);
            }

            if (signal is ClearCompletedTodosSignal)
            {
                return ClearCompletedTodosReducer(previousState, (ClearCompletedTodosSignal)signal);
            }

            if (signal is CompleteAllTodosSignal)
            {
                return CompleteAllTodosReducer(previousState, (CompleteAllTodosSignal)signal);
            }

            if (signal is CompleteTodoSignal)
            {
                return CompleteTodoReducer(previousState, (CompleteTodoSignal)signal);
            }

            if (signal is DeleteTodoSignal)
            {
                return DeleteTodoReducer(previousState, (DeleteTodoSignal)signal);
            }

            return previousState;
        }

        public static ApplicationState Execute(ApplicationState previousState, ISignal signal)
        {
            return new ApplicationState
            {
                Filter = signal is FilterTodosSignal ? ((FilterTodosSignal)signal).Filter : previousState.Filter,
                Todos = TodosReducer(previousState.Todos, signal)
            };
        }
    }
}
