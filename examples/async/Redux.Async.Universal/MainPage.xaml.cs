using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.System;

namespace Redux.Async.Universal
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            App.Store
                .Subscribe(state => RepositoriesItemsControl.ItemsSource = state.Repositories);
        }

        private async void SearchRepositoriesTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter)
                return;

            if (string.IsNullOrWhiteSpace(SearchRepositoriesTextBox.Text))
                return;

            App.Store.Dispatch(await ActionCreators.SearchRepositories(SearchRepositoriesTextBox.Text));
        }
    }
}
