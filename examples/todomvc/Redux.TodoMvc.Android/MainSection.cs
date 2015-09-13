using System;
using System.Collections.Generic;
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
                var list = FilterTodos(applicationState);

                _listView.Adapter = new ArrayAdapter<string>(_view.Context, global::Android.Resource.Layout.SimpleListItem1, list);
            });
        }

        private List<string> FilterTodos(ApplicationState applicationState)
        {
            return FilterTodos(applicationState.Todos, applicationState.Filter).Select(a => a.Text).ToList();
        }

        private IEnumerable<Todo> FilterTodos(IEnumerable<Todo> todos, TodosFilter filter)
        {
            if (filter == TodosFilter.Completed)
            {
                return todos.Where(x => x.IsCompleted);
            }

            if (filter == TodosFilter.InProgress)
            {
                return todos.Where(x => !x.IsCompleted);
            }

            return todos;
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