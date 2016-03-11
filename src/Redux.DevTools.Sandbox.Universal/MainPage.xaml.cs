using System;

namespace Redux.DevTools.Sandbox.Universal
{
    public class IncrementAction : IAction
    {

    }

    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            App.Store.Subscribe(counter => CounterTextBlock.Text = counter.ToString());
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.Store.Dispatch(new IncrementAction());
        }
    }
}
