using System.Linq;
using System;
using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Redux.TodoMvc.Android.Actions;
using Redux.TodoMvc.Android.States;

namespace Redux.TodoMvc.Android
{
    public class Header : Fragment
    {
        private EditText _editText;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActivityStore = ((MainActivity)this.Activity).Store;
        }

        public IStore<ApplicationState> ActivityStore { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Header, container, false);

            _editText = view.FindViewById<EditText>(Resource.Id.editTextId);
            _editText.TextChanged += EditText_TextChanged;
            
            var completeAllCheckBox = view.FindViewById<CheckBox>(Resource.Id.selectAllTodosCheckBox);
            completeAllCheckBox.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs args)
            {
                ActivityStore.Dispatch(new CompleteAllTodosAction
                {
                    IsCompleted = args.IsChecked
                });
            };

            ActivityStore.Subscribe(state =>
            {
                completeAllCheckBox.Visibility = state.Todos.Any() ? ViewStates.Visible : ViewStates.Invisible;
                completeAllCheckBox.Checked = state.Todos.All(x => x.IsCompleted);
            });
            
            return view;
        }

        private void EditText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text;
            if (text.Contains('\n'))
            {
                var textRemoveEnterChar = (text.ToString()).Trim();
                _editText.Text = string.Empty; ;

                ActivityStore.Dispatch(new AddTodoAction { Text = textRemoveEnterChar });
            }
        }
    }
}