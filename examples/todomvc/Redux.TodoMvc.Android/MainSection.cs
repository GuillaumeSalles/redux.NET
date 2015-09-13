using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Redux.TodoMvc.Android.States;

namespace Redux.TodoMvc.Android
{
    public class MainSection : Fragment
    {
        private ListView _listView;
        private View _view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActivityStore = ((MainActivity)this.Activity).Store;

            ActivityStore.Subscribe(applicationState =>
            {
                var list = applicationState.Todos.Select(a => a.Text).ToList();

                _listView.Adapter = new ArrayAdapter<string>(_view.Context, global::Android.Resource.Layout.SimpleListItem1, list);
            });
        }

        public IStore<ApplicationState> ActivityStore { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.MainSection, container, false);
            _listView = _view.FindViewById<ListView>(Resource.Id.listView1);
            return _view;
        }
    }
}