using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Redux.MultiThread.Counter.Universal
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            App.Store
                .ObserveOn(CoreDispatcherScheduler.Current)
                .Subscribe(counter => CounterRun.Text = counter.ToString());
        }

        public async void MultiThreadIncrementButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Task.WhenAll(Enumerable.Range(0, 1000)
                .Select(_ => Task.Factory.StartNew(() => App.Store.Dispatch(new IncrementAction()))));
        }
    }
}
