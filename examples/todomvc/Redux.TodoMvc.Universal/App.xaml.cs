using System.Collections.Immutable;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Redux.DevTools.Universal;
using Redux.TodoMvc.Universal.Reducers;
using Redux.TodoMvc.Universal.States;

namespace Redux.TodoMvc.Universal
{
    public sealed partial class App : Application
    {
        public static IStore<ApplicationState> Store { get; private set; }

        public App()
        {
            InitializeComponent();

            var initialState = new ApplicationState
            {
                Todos = ImmutableArray<Todo>.Empty,
                Filter = TodosFilter.All
            };

            //Store = new Store<ApplicationState>(initialState, ApplicationReducer.Execute);
            Store = new TimeMachineStore<ApplicationState>(initialState, ApplicationReducer.Execute);
        }
        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            
            if (rootFrame == null)
            {
                //rootFrame = new Frame();
                rootFrame = new DevFrame
                {
                    TimeMachineStore = (IStore<TimeMachineState>)Store
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
