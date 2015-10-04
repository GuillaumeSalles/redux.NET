using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Redux.TodoMvc.Core.Actions;
using Redux.TodoMvc.Core.States;

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

                _listView.Adapter = new ListItemAdapter(this.Activity, list, ActivityStore);
            });
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

        public IStore<ApplicationState> ActivityStore { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.MainSection, container, false);
            _listView = _view.FindViewById<ListView>(Resource.Id.listView1);
            return _view;
        }
    }

    public class ListItemAdapter : BaseAdapter<Todo>
    {
        List<Todo> items;
        private readonly IStore<ApplicationState> _activityStore;
        Activity context;
        public ListItemAdapter(Activity context, List<Todo> items, IStore<ApplicationState> activityStore)
            : base()
        {
            this.context = context;
            this.items = items;
            _activityStore = activityStore;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Todo this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            var view = context.LayoutInflater.Inflate(Resource.Layout.ListViewItem, null);

            var checkBox = view.FindViewById<CheckBox>(Resource.Id.checkbox);

            checkBox.Checked = item.IsCompleted;
            checkBox.CheckedChange += delegate
            {
                _activityStore.Dispatch(new CompleteTodoAction()
                {
                    TodoId = item.Id
                });
            };


            view.FindViewById<TextView>(Resource.Id.todoTextItem).Text = item.Text;

            view.FindViewById<Button>(Resource.Id.RemoveButton).Click += delegate
            {
                _activityStore.Dispatch(new DeleteTodoAction
                {
                    TodoId = item.Id
                });
            };

            return view;
        }
    }
}