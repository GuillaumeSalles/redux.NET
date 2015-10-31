using System;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.Immutable;
using Windows.UI.Xaml;
using Redux.TodoMvc.Actions;
using Redux.TodoMvc.States;
using Redux.TodoMvc.ViewModels;

namespace Redux.TodoMvc.Universal
{
    public sealed partial class Footer
    {
        public Footer()
        {
            this.InitializeComponent();

            App.Store
                .Select(Selectors.MakeFooterViewModel)
                .Subscribe(viewModel =>
                {
                    ActiveTodoCounterTextBlock.Text = viewModel.ActiveTodosCounterMessage;

                    ClearActiveTodoButton.Visibility = viewModel.ClearTodosIsVisible ? 
                        Visibility.Visible : 
                        Visibility.Collapsed;

                    CheckFilter(viewModel.SelectedFilter);
                });
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

        private void ClearActiveTodoButton_Click(object sender, RoutedEventArgs e)
        {
            App.Store.Dispatch(new ClearCompletedTodosAction());
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
        
        private void FilterTodos(TodosFilter filter)
        {
            App.Store.Dispatch(new FilterTodosAction
            {
                Filter = filter
            });
        }
    }
}
