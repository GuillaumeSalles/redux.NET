using Windows.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace Redux.Sandbox.Universal
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            App.CounterStore.Subscribe(counter => CounterRun.Text = counter.ToString());
        }

        private void IncrementButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.CounterStore.Dispatch(new IncrementAction());
        }

        private void DecrementButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.CounterStore.Dispatch(new DecrementAction());
        }

        private async void IncrementIfOddButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var counter = await App.CounterStore.FirstAsync();
            if (counter % 2 == 1)
            {
                App.CounterStore.Dispatch(new IncrementAction());
            }
        }

        private async void IncrementAsyncButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Task.Delay(1000);
            App.CounterStore.Dispatch(new IncrementAction());
        }
    }
}
