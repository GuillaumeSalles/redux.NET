using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using Redux.TodoMvc.States;

namespace Redux.TodoMvc.Forms
{
    public partial class MainSection : ContentView
    {
        public MainSection()
        {
            this.InitializeComponent();

            App.Store.Subscribe((ApplicationState state) =>
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

