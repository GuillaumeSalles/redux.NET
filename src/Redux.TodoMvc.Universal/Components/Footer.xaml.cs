using System;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.Immutable;
using Windows.UI.Xaml;
using Redux.TodoMvc.Universal.Signals;

namespace Redux.TodoMvc.Universal.Components
{
    public sealed partial class Footer
    {
        public Footer()
        {
            this.InitializeComponent();

            App.Store.Subscribe(state =>
            {
                ActiveTodoCounterTextBlock.Text = GetActiveTodosCounterMessage(state.Todos);

                ClearActiveTodoButton.Visibility = ClearActiveTodoButtonVisibility(state.Todos);

                CheckFilter(state.Filter);
            });
        }

        private string GetActiveTodosCounterMessage(ImmutableArray<Todo> todos)
        {
            var activeTodoCount = todos.Count(todo => !todo.IsCompleted);
            var itemWord = activeTodoCount <= 1 ? "item" : "items";
            return activeTodoCount + " " + itemWord + " left";
        }

        private void CheckFilter(TodosFilter filter)
        {
            if(filter == TodosFilter.All)
            {
                AllFilter.IsChecked = true;
            }
            else if(filter == TodosFilter.Completed)
            {
                CompletedFilter.IsChecked = true;
            }
            else if(filter == TodosFilter.InProgress)
            {
                InProgressFilter.IsChecked = true;
            }
        }

        private Visibility ClearActiveTodoButtonVisibility(ImmutableArray<Todo> todos)
        {
            return todos.Any(todo => todo.IsCompleted) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClearActiveTodoButton_Click(object sender, RoutedEventArgs e)
        {
            App.Store.Dispatch(new ClearCompletedTodosSignal());
        }

        private void FilterTodos(TodosFilter filter)
        {
            App.Store.Dispatch(new FilterTodosSignal
            {
                Filter = filter
            });
        }

        private void AllFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterTodos(TodosFilter.All);
        }

        private void InProgressFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterTodos(TodosFilter.InProgress);
        }

        private void CompletedFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterTodos(TodosFilter.Completed);
        }
    }
}
