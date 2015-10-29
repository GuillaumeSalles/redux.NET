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
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Footer, container, false);
            var textView = view.FindViewById<TextView>(Resource.Id.numberOfTodosRemaining);

            var clearCompletedButton = view.FindViewById<Button>(Resource.Id.clearCompletedButton);
            clearCompletedButton.Click += delegate
            {
                MainActivity.Store.Dispatch(new ClearCompletedTodosAction());
            };

            var radioButtonGroup = view.FindViewById<RadioGroup>(Resource.Id.radioGroup);

            var allRadioButton = view.FindViewById<RadioButton>(Resource.Id.allRadioButton);
            var activeRadioButton = view.FindViewById<RadioButton>(Resource.Id.activeRadioButton);
            var completedRadioButton = view.FindViewById<RadioButton>(Resource.Id.completedRadioButton);


            allRadioButton.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs args)
            {
                if (args.IsChecked) AllFilter_Click();
            };

            activeRadioButton.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs args)
            {
                if (args.IsChecked) InProgressFilter_Click();
            };

            completedRadioButton.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs args)
            {
                if (args.IsChecked) CompletedFilter_Click();
            };

            MainActivity.Store.Subscribe(state =>
            {
                textView.Text = BuildText(state.Todos);
                clearCompletedButton.Visibility = ClearActiveTodoButtonVisibility(state.Todos);
                radioButtonGroup.Visibility = state.Todos.Any() ? ViewStates.Visible : ViewStates.Invisible;
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