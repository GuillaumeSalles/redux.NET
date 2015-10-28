using System;

using Xamarin.Forms;
using Redux;
using System.Collections.Immutable;

namespace todoRedux
{
    public class App : Application
    {
        public static IStore<ApplicationState> Store { get; private set; }

        public App ()
        {
            var initialState = new ApplicationState
            {
                Todos = ImmutableArray<Todo>.Empty,
                Filter = TodosFilter.All
            };

            Store = new Store<ApplicationState>(initialState, ApplicationReducer.Execute);
            //Store = new TimeMachineStore<ApplicationState>(initialState, ApplicationReducer.Execute);

            MainPage = new ContentPage {
                Content = new StackLayout {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            XAlign = TextAlignment.Center,
                            Text = "Welcome to Xamarin Forms!"
                        }
                    }
                }
            };
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}

