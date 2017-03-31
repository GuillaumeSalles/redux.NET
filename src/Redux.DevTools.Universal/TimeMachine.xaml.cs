using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Newtonsoft.Json;

namespace Redux.DevTools.Universal
{
    public sealed partial class TimeMachine : UserControl
    {
        private TimeMachineState _lastState;

        public IStore<TimeMachineState> TimeMachineStore
        {
            get { return (IStore<TimeMachineState>)GetValue(TimeMachineStoreProperty); }
            set { SetValue(TimeMachineStoreProperty, value); }
        }

        public static readonly DependencyProperty TimeMachineStoreProperty =
            DependencyProperty.Register("TimeMachineStore", typeof(IStore<TimeMachineState>), typeof(TimeMachine), new PropertyMetadata(null, OnTimeMachineStoreChanged));

        private static void OnTimeMachineStoreChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var timeMachine = (TimeMachine)sender;

            var oldTimeMachineStore = args.OldValue as IStore<TimeMachineState>;
            if (oldTimeMachineStore != null)
            {
                oldTimeMachineStore.StateChanged -= timeMachine.OnStateChange;
            }

            var newTimeMachineStore = args.NewValue as IStore<TimeMachineState>;
            if (newTimeMachineStore != null)
            {
                newTimeMachineStore.StateChanged -= timeMachine.OnStateChange;
            }
        }

        public TimeMachine()
        {
            this.InitializeComponent();
        }

        private void OnStateChange(TimeMachineState state)
        {
            _lastState = state;

            Shield.Visibility = state.IsPaused ? Visibility.Visible : Visibility.Collapsed;

            PauseButton.Visibility = state.IsPaused ? Visibility.Collapsed : Visibility.Visible;
            PlayButton.Visibility = state.IsPaused ? Visibility.Visible : Visibility.Collapsed;

            ActionPositionsSlider.Value = state.Position;
            ActionPositionsSlider.Maximum = state.Actions.Count;

            if (state.Position <= 0)
            {
                CurrentActionTypeTextBlock.Text = string.Empty;
                CurrentActionDescription.Text = string.Empty;
            }
            else
            {
                var currentAction = state.Actions[state.Position - 1];
                CurrentActionTypeTextBlock.Text = currentAction.GetType().Name;
                CurrentActionDescription.Text = JsonConvert.SerializeObject(currentAction, Formatting.Indented);
            }
        }

        private void ActionPositionsSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (TimeMachineStore == null
                || ActionPositionsSlider.Value == _lastState.Position)
            {
                return;
            }

            TimeMachineStore.Dispatch(new SetTimeMachinePositionAction
            {
                Position = (int)ActionPositionsSlider.Value
            });
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeMachineStore == null)
                return;

            TimeMachineStore.Dispatch(new ResumeTimeMachineAction());
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeMachineStore == null)
                return;

            TimeMachineStore.Dispatch(new PauseTimeMachineAction());
        }
    }
}