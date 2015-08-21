using Redux.TodoMvc.Universal.Signals;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System;
using System.Linq;
using Windows.UI.Xaml;

namespace Redux.TodoMvc.Universal.Components
{
    public sealed partial class Header : UserControl
    {
        public Header()
        {
            this.InitializeComponent();

            App.Store.Subscribe(state =>
            {
                MarkAllCheckBox.Visibility = state.Todos.Any() ? Visibility.Visible : Visibility.Collapsed; 
                MarkAllCheckBox.IsChecked = state.Todos.All(x => x.IsMarked);
            });
        }

        private void TodoInputTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter)
                return;

            App.Store.Dispatch(new AddTodoSignal
            {
                Text = TodoInputTextBox.Text
            });

            TodoInputTextBox.Text = string.Empty;
        }

        private void MarkAllCheckBox_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.Store.Dispatch(new MarkAllSignal
            {
                IsMarked = MarkAllCheckBox.IsChecked.Value
            });
        }
    }
}
