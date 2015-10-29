using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Redux.TodoMvc.States;

namespace Redux.TodoMvc.Android
{
    public class MainSection : Fragment
    {
        private ListView _listView;
        private View _view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.MainSection, container, false);
            _listView = _view.FindViewById<ListView>(Resource.Id.listView1);


            MainActivity.Store.Subscribe(applicationState =>
            {
                var list = FilterTodos(applicationState);

                _listView.Adapter = new ListItemAdapter(this.Activity, list);
            });

            return _view;
        }

        private List<Todo> FilterTodos(ApplicationState applicationState)
        {
            return FilterTodos(applicationState.Todos, applicationState.Filter).ToList();
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
    }
}