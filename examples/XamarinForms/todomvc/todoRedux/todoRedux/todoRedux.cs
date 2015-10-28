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

			//Store = new Store<ApplicationState>(initialState, ApplicationReducer.Execute);
			Store = new TimeMachineStore<ApplicationState> (initialState, ApplicationReducer.Execute);

			//MainPage = new MainPage ();
			NavigationPage navPage = new NavigationPage (
				                         new DevFrame () {
					TimeMachineStore = (IStore<TimeMachineState>)Store
				});
			MainPage = navPage;
			navPage.Navigation.PushAsync (new MainPage ());
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

