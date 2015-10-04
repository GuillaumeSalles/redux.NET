using System.Collections.Immutable;
using Android.App;
using Android.OS;
using Redux.TodoMvc.Core.Reducers;
using Redux.TodoMvc.Core.States;

namespace Redux.TodoMvc.Android
{
    [Activity(Label = "Redux.TodoMvc.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public  IStore<ApplicationState> Store { get; private set; }


        public MainActivity()
        {
            var initialState = new ApplicationState
            {
                Todos = ImmutableArray<Todo>.Empty,
                Filter = TodosFilter.All
            };

            Store = new Store<ApplicationState>(initialState, ApplicationReducer.Execute);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
        }
        
    }
}

