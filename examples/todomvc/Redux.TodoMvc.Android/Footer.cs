using System;
using System.Collections.Immutable;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Redux.TodoMvc.Actions;
using Redux.TodoMvc.States;

namespace Redux.TodoMvc.Android
{
    public class Footer : Fragment
    {
        private TextView _textView;
        private RadioButton _allRadioButton;
        private RadioButton _activeRadioButton;
        private RadioButton _completedRadioButton;
        private Button _clearCompletedButton;
        private RadioGroup _radioButtonGroup;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Footer, container, false);
            _textView = view.FindViewById<TextView>(Resource.Id.numberOfTodosRemaining);

            _clearCompletedButton = view.FindViewById<Button>(Resource.Id.clearCompletedButton);
            _clearCompletedButton.Click += delegate
            {
                MainActivity.Store.Dispatch(new ClearCompletedTodosAction());
            };

            _radioButtonGroup = view.FindViewById<RadioGroup>(Resource.Id.radioGroup);

            _allRadioButton = view.FindViewById<RadioButton>(Resource.Id.allRadioButton);
            _activeRadioButton = view.FindViewById<RadioButton>(Resource.Id.activeRadioButton);
            _completedRadioButton = view.FindViewById<RadioButton>(Resource.Id.completedRadioButton);


            _allRadioButton.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs args)
            {
                if (args.IsChecked) AllFilter_Click();
            };

            _activeRadioButton.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs args)
            {
                if (args.IsChecked) InProgressFilter_Click();
            };

            _completedRadioButton.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs args)
            {
                if (args.IsChecked) CompletedFilter_Click();
            };

            MainActivity.Store.Subscribe(state =>
            {
                _textView.Text = BuildText(state.Todos);
                _clearCompletedButton.Visibility = ClearActiveTodoButtonVisibility(state.Todos);
                _radioButtonGroup.Visibility = state.Todos.Any() ? ViewStates.Visible : ViewStates.Invisible;
            });

            return view;
        }

        private string BuildText(ImmutableArray<Todo> todos)
        {
            if (!todos.Any()) return string.Empty;

            if (todos.Count() == 1) return "1 item left";

            return $"{todos.Count()} items left";
        }

        private void FilterTodos(TodosFilter filter)
        {
            MainActivity.Store.Dispatch(new FilterTodosAction
            {
                Filter = filter
            });
        }

        private void AllFilter_Click()
        {
            FilterTodos(TodosFilter.All);
        }

        private void InProgressFilter_Click()
        {
            FilterTodos(TodosFilter.InProgress);
        }

        private void CompletedFilter_Click()
        {
            FilterTodos(TodosFilter.Completed);
        }

        private ViewStates ClearActiveTodoButtonVisibility(ImmutableArray<Todo> todos)
        {
            return todos.Any(todo => todo.IsCompleted) ? ViewStates.Visible : ViewStates.Invisible;
        }
    }
}