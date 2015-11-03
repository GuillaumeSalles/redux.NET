using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Redux.TodoMvc.Actions;
using Redux.TodoMvc.States;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.TodoMvc.Android
{
    public class Footer : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Footer, container, false);

            var textView = view.FindViewById<TextView>(Resource.Id.numberOfTodosRemaining);
            var clearCompletedButton = view.FindViewById<Button>(Resource.Id.clearCompletedButton);
            var radioButtonGroup = view.FindViewById<RadioGroup>(Resource.Id.radioGroup);
            var allRadioButton = view.FindViewById<RadioButton>(Resource.Id.allRadioButton);
            var activeRadioButton = view.FindViewById<RadioButton>(Resource.Id.activeRadioButton);
            var completedRadioButton = view.FindViewById<RadioButton>(Resource.Id.completedRadioButton);

            clearCompletedButton.Click += (sender, args) => MainActivity.Store.Dispatch(new ClearCompletedTodosAction());

            allRadioButton.CheckedChange += (sender, args) =>
            {
                if (args.IsChecked)
                {
                    FilterTodos(TodosFilter.All);
                };
            };

            activeRadioButton.CheckedChange += (sender, args) =>
            {
                if (args.IsChecked)
                {
                    FilterTodos(TodosFilter.InProgress);
                }
            };

            completedRadioButton.CheckedChange += (sender, args) =>
            {
                if (args.IsChecked)
                {
                    FilterTodos(TodosFilter.Completed);
                }
            };

            MainActivity.Store
                .Select(Selectors.MakeFooterViewModel)
                .Subscribe(viewModel =>
                {
                    textView.Text = viewModel.ActiveTodosCounterMessage;
                    clearCompletedButton.Visibility = viewModel.ClearTodosIsVisible ? ViewStates.Visible : ViewStates.Invisible;
                    radioButtonGroup.Visibility = viewModel.AreFiltersVisible ? ViewStates.Visible : ViewStates.Invisible;
                });

            return view;
        }
        
        private void FilterTodos(TodosFilter filter)
        {
            MainActivity.Store.Dispatch(new FilterTodosAction
            {
                Filter = filter
            });
        }
    }
}
