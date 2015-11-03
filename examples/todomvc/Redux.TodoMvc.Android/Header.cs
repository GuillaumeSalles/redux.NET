using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Redux.TodoMvc.Actions;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.TodoMvc.Android
{
    public class Header : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Header, container, false);

            var editText = view.FindViewById<EditText>(Resource.Id.editTextId);
            editText.TextChanged += EditText_TextChanged;

            var completeAllCheckBox = view.FindViewById<CheckBox>(Resource.Id.selectAllTodosCheckBox);
            completeAllCheckBox.CheckedChange += (sender, args) =>
            {
                MainActivity.Store.Dispatch(new CompleteAllTodosAction
                {
                    IsCompleted = args.IsChecked
                });
            };

            MainActivity.Store
                .Select(Selectors.MakeHeaderViewModel)
                .Subscribe(viewModel =>
                {
                    completeAllCheckBox.Visibility = viewModel.CompleteAllIsVisible ? ViewStates.Visible : ViewStates.Invisible;
                    completeAllCheckBox.Checked = viewModel.CompleteAllIsChecked;
                });

            return view;
        }

        private void EditText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text;
            if (text.Contains('\n'))
            {
                var textRemoveEnterChar = (text.ToString()).Trim();
                ((EditText)sender).Text = string.Empty;

                MainActivity.Store.Dispatch(new AddTodoAction { Text = textRemoveEnterChar });
            }
        }
    }
}
