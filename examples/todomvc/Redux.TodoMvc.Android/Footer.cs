using System;
using System.Collections.Immutable;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Redux.TodoMvc.Android.States;

namespace Redux.TodoMvc.Android
{
    public class Footer : Fragment
    {
        private TextView _textView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActivityStore = ((MainActivity)this.Activity).Store;


            ActivityStore.Subscribe(a => { _textView.Text = BuildText(a.Todos, a.Filter); });
        }

        private string BuildText(ImmutableArray<Todo> todos, TodosFilter filter)
        {
            if (!todos.Any()) return string.Empty;

            if (todos.Count() == 1) return "1 item left";

            return $"{todos.Count()} items left";
        }

        public IStore<ApplicationState> ActivityStore { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Footer, container, false);

            _textView = view.FindViewById<TextView>(Resource.Id.numberOfTodosRemaining);


            return view;
        }
    }
}