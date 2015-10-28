using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.Immutable;
using System.Linq;

namespace todoRedux
{
    public partial class Footer : ContentView
    {
        public Footer()
        {
            this.InitializeComponent();

            App.Store.Subscribe(state =>
                {
                    ActiveTodoCounterTextBlock.Text = GetActiveTodosCounterMessage(state.Todos);

                    ClearActiveTodoButton.IsVisible = ClearActiveTodoButtonVisibility(state.Todos);

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
                AllFilter.IsToggled = true;
            }
            else if(filter == TodosFilter.Completed)
            {
                CompletedFilter.IsToggled = true;
            }
            else if(filter == TodosFilter.InProgress)
            {
                InProgressFilter.IsToggled = true;
            }
        }

        private bool ClearActiveTodoButtonVisibility(ImmutableArray<Todo> todos)
        {
            return todos.Any(todo => todo.IsCompleted) ? true : false;
        }

        private void ClearActiveTodoButton_Click(object sender, EventArgs e)
        {
            App.Store.Dispatch(new ClearCompletedTodosAction());
        }

        private void FilterTodos(TodosFilter filter)
        {
            App.Store.Dispatch(new FilterTodosAction
                {
                    Filter = filter
                });
        }

        private void AllFilter_Click(object sender, EventArgs e)
        {
            FilterTodos(TodosFilter.All);
        }

        private void InProgressFilter_Click(object sender, EventArgs e)
        {
            FilterTodos(TodosFilter.InProgress);
        }

        private void CompletedFilter_Click(object sender, EventArgs e)
        {
            FilterTodos(TodosFilter.Completed);
        }

    }
}

