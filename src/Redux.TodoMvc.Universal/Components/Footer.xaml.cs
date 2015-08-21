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
            });
        }

        private string GetActiveTodosCounterMessage(ImmutableArray<Todo> todos)
        {
            var activeTodoCount = todos.Count(todo => !todo.IsMarked);
            var itemWord = activeTodoCount <= 1 ? "item" : "items";
            return activeTodoCount + " " + itemWord + " left";
        }

        private Visibility ClearActiveTodoButtonVisibility(ImmutableArray<Todo> todos)
        {
            return todos.Any(todo => todo.IsMarked) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClearActiveTodoButton_Click(object sender, RoutedEventArgs e)
        {
            App.Store.Dispatch(new ClearMarkedSignal());
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

        private void MarkedFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterTodos(TodosFilter.Marked);
        }
    }
}
