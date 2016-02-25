using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Redux.Async.Universal
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            App.Store
                .Subscribe(state =>
                {
                    RepositoriesItemsControl.ItemsSource = state.Repositories;
                    IsSearchingBorder.Visibility = state.IsSearching ? Visibility.Visible : Visibility.Collapsed;
                });
        }
        
        private async void SearchButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await App.Store.Dispatch(ActionCreators.SearchRepositories(SearchRepositoriesTextBox.Text));
        }
    }
}
