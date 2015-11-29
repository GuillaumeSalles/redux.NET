using Redux.TodoMvc.States;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Redux.TodoMvc.Forms
{
    public class App : Application
    {
        public static IStore<ApplicationState> Store { get; private set; }

        public App()
        {
            var initialState = new ApplicationState
            {
                Todos = ImmutableArray<Todo>.Empty,
                Filter = TodosFilter.All
            };

            var masterDetail = new MasterDetailPage();

            Store = new Store<ApplicationState>(initialState, Reducers.ReduceApplication);

            masterDetail.Detail =
                new NavigationPage(
                    new MainPage() { Title = "Todo List" }
                );

            MainPage = masterDetail;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
