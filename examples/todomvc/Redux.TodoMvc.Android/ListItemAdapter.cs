using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Redux.TodoMvc.Actions;
using Redux.TodoMvc.States;

namespace Redux.TodoMvc.Android
{
    public class ListItemAdapter : BaseAdapter<Todo>
    {
        private readonly List<Todo> _items;
        private readonly Activity _context;

        public ListItemAdapter(Activity context, List<Todo> items)
        {
            _context = context;
            _items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Todo this[int position] => _items[position];

        public override int Count => _items.Count;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];
            var view = _context.LayoutInflater.Inflate(Resource.Layout.ListViewItem, null);

            var checkBox = view.FindViewById<CheckBox>(Resource.Id.checkbox);

            checkBox.Checked = item.IsCompleted;
            checkBox.CheckedChange += delegate
            {
                MainActivity.Store.Dispatch(new CompleteTodoAction()
                {
                    TodoId = item.Id
                });
            };
            
            view.FindViewById<TextView>(Resource.Id.todoTextItem).Text = item.Text;

            view.FindViewById<Button>(Resource.Id.RemoveButton).Click += delegate
            {
                MainActivity.Store.Dispatch(new DeleteTodoAction
                {
                    TodoId = item.Id
                });
            };

            return view;
        }
    }
}