using System;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;

namespace Redux.TodoMvc.Universal
{
    public sealed partial class MainSection : UserControl
    {
        public MainSection()
        {
            this.InitializeComponent();

            App.Store
                .Select(Selectors.GetFilteredTodos)
                .Subscribe(todos => TodosItemsControl.ItemsSource = todos);
        }
    }
}
