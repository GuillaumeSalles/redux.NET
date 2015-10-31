using Redux.TodoMvc.States;
using Redux.TodoMvc.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Redux.TodoMvc
{
    public static class Selectors
    {
        public static IEnumerable<Todo> GetFilteredTodos(ApplicationState state)
        {
            if (state.Filter == TodosFilter.Completed)
            {
                return state.Todos.Where(x => x.IsCompleted);
            }

            if (state.Filter == TodosFilter.InProgress)
            {
                return state.Todos.Where(x => !x.IsCompleted);
            }

            return state.Todos;
        }

        public static HeaderViewModel MakeHeaderViewModel(ApplicationState state)
        {
            return new HeaderViewModel
            {
                CompleteAllIsChecked = state.Todos.All(x => x.IsCompleted),
                CompleteAllIsVisible = state.Todos.Any()
            };
        }

        public static FooterViewModel MakeFooterViewModel(ApplicationState state)
        {
            return new FooterViewModel
            {
                ClearTodosIsVisible = state.Todos.Any(todo => todo.IsCompleted),
                ActiveTodosCounterMessage = GetActiveTodosCounterMessage(state.Todos),
                SelectedFilter = state.Filter,
                AreFiltersVisible = state.Todos.Any()
            };
        }

        public static string GetActiveTodosCounterMessage(IEnumerable<Todo> todos)
        {
            var activeTodoCount = todos.Count(todo => !todo.IsCompleted);
            var itemWord = activeTodoCount <= 1 ? "item" : "items";
            return activeTodoCount + " " + itemWord + " left";
        }
    }
}
