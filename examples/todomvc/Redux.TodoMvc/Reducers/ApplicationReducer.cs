using System;
using System.Collections.Immutable;
using System.Linq;
using Redux.TodoMvc.Actions;
using Redux.TodoMvc.States;

namespace Redux.TodoMvc.Reducers
{
    public static class ApplicationReducer
    {
        public static ImmutableArray<Todo> AddTodoReducer(ImmutableArray<Todo> previousState, AddTodoAction action)
        {
            return previousState
                .Insert(0, new Todo
                {
                    Id = Guid.NewGuid(),
                    Text = action.Text
                });
        }

        public static ImmutableArray<Todo> ClearCompletedTodosReducer(ImmutableArray<Todo> previousState, ClearCompletedTodosAction action)
        {
            return previousState.RemoveAll(todo => todo.IsCompleted);
        }

        public static ImmutableArray<Todo> CompleteAllTodosReducer(ImmutableArray<Todo> previousState, CompleteAllTodosAction action)
        {
            return previousState
                .Select(x => new Todo
                {
                    Id = x.Id,
                    Text = x.Text,
                    IsCompleted = action.IsCompleted
                })
                .ToImmutableArray();
        }

        public static ImmutableArray<Todo> CompleteTodoReducer(ImmutableArray<Todo> previousState, CompleteTodoAction action)
        {
            var todoToEdit = previousState.First(todo => todo.Id == action.TodoId);

            return previousState
                .Replace(todoToEdit, new Todo
                {
                    Id = todoToEdit.Id,
                    Text = todoToEdit.Text,
                    IsCompleted = !todoToEdit.IsCompleted
                });
        }

        public static ImmutableArray<Todo> DeleteTodoReducer(ImmutableArray<Todo> previousState, DeleteTodoAction action)
        {
            var todoToDelete = previousState.First(todo => todo.Id == action.TodoId);

            return previousState.Remove(todoToDelete);
        }

        public static ImmutableArray<Todo> TodosReducer(ImmutableArray<Todo> previousState, IAction action)
        {
            if (action is AddTodoAction)
            {
                return AddTodoReducer(previousState, (AddTodoAction)action);
            }

            if (action is ClearCompletedTodosAction)
            {
                return ClearCompletedTodosReducer(previousState, (ClearCompletedTodosAction)action);
            }

            if (action is CompleteAllTodosAction)
            {
                return CompleteAllTodosReducer(previousState, (CompleteAllTodosAction)action);
            }

            if (action is CompleteTodoAction)
            {
                return CompleteTodoReducer(previousState, (CompleteTodoAction)action);
            }

            if (action is DeleteTodoAction)
            {
                return DeleteTodoReducer(previousState, (DeleteTodoAction)action);
            }

            return previousState;
        }

        public static ApplicationState Execute(ApplicationState previousState, IAction action)
        {
            return new ApplicationState
            {
                Filter = action is FilterTodosAction ? ((FilterTodosAction)action).Filter : previousState.Filter,
                Todos = TodosReducer(previousState.Todos, action)
            };
        }
    }
}
