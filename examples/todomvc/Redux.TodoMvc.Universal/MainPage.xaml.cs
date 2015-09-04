using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Redux.TodoMvc.Universal
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            App.Store.Subscribe(state =>
            {
                Footer.Visibility = state.Todos.Any() ? Visibility.Visible : Visibility.Collapsed;
            });
        }
    }
}
