using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Redux.DevTools.WPF
{
    public partial class TimeMachine : UserControl
    {
        private IDisposable _storeSubscription;
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
                timeMachine._storeSubscription.Dispose();
            }

            var newTimeMachineStore = args.NewValue as IStore<TimeMachineState>;
            if (newTimeMachineStore != null)
            {
                timeMachine.SubscribeToTimeMachineStore();
            }
        }

        public TimeMachine()
        {
            this.InitializeComponent();
        }

        private void SubscribeToTimeMachineStore()
        {
            _storeSubscription = TimeMachineStore.Subscribe(OnStateChange);
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

        private void ActionPositionsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
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
