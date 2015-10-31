using Redux.TodoMvc.Actions;
using System;
using System.Linq;
using System.Reactive.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Redux.TodoMvc.Universal
{
    public sealed partial class Header : UserControl
    {
        public Header()
        {
            this.InitializeComponent();

            App.Store
                .Select(Selectors.MakeHeaderViewModel)
                .Subscribe(viewModel =>
                {
                    CompleteAllCheckBox.Visibility = viewModel.CompleteAllIsVisible ? Visibility.Visible : Visibility.Collapsed; 
                    CompleteAllCheckBox.IsChecked = viewModel.CompleteAllIsChecked;
                });
        }

        private void TodoInputTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter)
                return;

            App.Store.Dispatch(new AddTodoAction
            {
                Text = TodoInputTextBox.Text
            });

            TodoInputTextBox.Text = string.Empty;
        }

        private void CompleteAllCheckBox_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.Store.Dispatch(new CompleteAllTodosAction
            {
                IsCompleted = CompleteAllCheckBox.IsChecked.Value
            });
        }
    }
}
