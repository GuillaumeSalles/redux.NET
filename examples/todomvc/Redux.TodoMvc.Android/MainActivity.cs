using System;
using System.Collections.Immutable;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Redux.TodoMvc.Android.Reducers;
using Redux.TodoMvc.Android.States;

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

