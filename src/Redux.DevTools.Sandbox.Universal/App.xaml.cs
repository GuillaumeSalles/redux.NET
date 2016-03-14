using System.Diagnostics;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Redux.StoreEnhancers;

namespace Redux.DevTools.Sandbox.Universal
{
    sealed partial class App
    {
        private Instruments _instruments = new Instruments();

        public static IStore<int> Store { get; set; }

        public App()
        {
            InitializeComponent();
            
            Middleware<int> middleware = store => next => action =>
            {
                var dispatchedAction = next(action);
                Debug.WriteLine("Action of type {0} as been dispatched", dispatchedAction.GetType());
                return dispatchedAction;
            };

            Store = StoreFactory.Create(
                (state, action) => state + 1,
                Compose(
                    ApplyMiddleware(middleware),
                    _instruments.Enhancer<int>()));
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new DevTools.Universal.DevFrame
                {
                    DevToolsStore = _instruments.LiftedStore
                };

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }

            Window.Current.Activate();
        }
    }
}
