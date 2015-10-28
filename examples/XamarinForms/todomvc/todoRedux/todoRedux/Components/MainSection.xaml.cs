using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace todoRedux
{
    public partial class MainSection : ContentView
    {
        public MainSection()
        {
            this.InitializeComponent();

            App.Store.Subscribe(state =>
                {
                    TodosItemsControl.ItemsSource = FilterTodos(state.Todos,state.Filter);
                });
        }

        private IEnumerable<Todo> FilterTodos(IEnumerable<Todo> todos, TodosFilter filter)
        {
            if(filter == TodosFilter.Completed)
            {
                return todos.Where(x => x.IsCompleted);
            }

            if(filter == TodosFilter.InProgress)
            {
                return todos.Where(x => !x.IsCompleted);
            }

            return todos;
        }
    }
}

