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
                CompleteAllCheckBox.Visibility = state.Todos.Any() ? Visibility.Visible : Visibility.Collapsed; 
                CompleteAllCheckBox.IsChecked = state.Todos.All(x => x.IsCompleted);
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

        private void CompleteAllCheckBox_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.Store.Dispatch(new CompleteAllTodosSignal
            {
                IsCompleted = CompleteAllCheckBox.IsChecked.Value
            });
        }
    }
}
