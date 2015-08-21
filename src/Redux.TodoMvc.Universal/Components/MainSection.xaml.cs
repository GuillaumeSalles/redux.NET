using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Redux.TodoMvc.Universal.Components
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
            if(filter == TodosFilter.Marked)
            {
                return todos.Where(x => x.IsMarked);
            }

            if(filter == TodosFilter.InProgress)
            {
                return todos.Where(x => !x.IsMarked);
            }

            return todos;
        }
    }
}
