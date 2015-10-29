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
			var initialState = new ApplicationState {
				Todos = ImmutableArray<Todo>.Empty,
				Filter = TodosFilter.All
			};

//			//MainPage = new MainPage ();
//			NavigationPage navPage = new NavigationPage (
//				                         new DevFrame () {
//					TimeMachineStore = (IStore<TimeMachineState>)Store,
//					//Content = new TimeMachine(),
//				});
//
            var masterDetail = new MasterDetailPage ();

            bool enableTimeMachine = true;
            if (enableTimeMachine) {
                Store = new TimeMachineStore<ApplicationState> (initialState, ApplicationReducer.Execute);

                masterDetail.Master = 
                    new NavigationPage (
                        new DevFrame ((IStore<TimeMachineState>)Store) { 
                            Title = "Time Machine"
                        }
                    ) { Title = "Menu" };
                
            } else {
                Store = new Store<ApplicationState>(initialState, ApplicationReducer.Execute);
            }

            masterDetail.Detail = 
                new NavigationPage (
                    new MainPage () { Title = "Todo List" }
                );

            MainPage = masterDetail;
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

