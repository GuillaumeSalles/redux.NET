using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.TodoMvc.Android
{
    public class MainSection : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.MainSection, container, false);
            var todosListView = view.FindViewById<ListView>(Resource.Id.listView1);
            
            MainActivity.Store
                .Select(Selectors.GetFilteredTodos)
                .Subscribe(todos => todosListView.Adapter = new ListItemAdapter(Activity, todos.ToList()));

            return view;
        }
    }
}