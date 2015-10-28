using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Redux.TodoMvc.States;

namespace Redux.TodoMvc.Universal
{
    public sealed partial class MainSection : UserControl
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
