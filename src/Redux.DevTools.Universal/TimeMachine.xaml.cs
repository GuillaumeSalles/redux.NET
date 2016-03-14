using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Newtonsoft.Json;

namespace Redux.DevTools.Universal
{
    public sealed partial class TimeMachine : UserControl
    {
        private IDisposable _storeSubscription;
        private DevToolsState _lastState;

        public IStore<DevToolsState> DevToolsStore
        {
            get { return (IStore<DevToolsState>)GetValue(DevToolsStoreProperty); }
            set { SetValue(DevToolsStoreProperty, value); }
        }

        public static readonly DependencyProperty DevToolsStoreProperty =
            DependencyProperty.Register("DevToolsStore", typeof(IStore<DevToolsState>), typeof(TimeMachine), new PropertyMetadata(null, OnTimeMachineStoreChanged));
        
        private static void OnTimeMachineStoreChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var timeMachine = (TimeMachine)sender;

            var oldTimeMachineStore = args.OldValue as IStore<DevToolsState>;
            if(oldTimeMachineStore != null)
            {
                timeMachine._storeSubscription.Dispose();
            }

            var newTimeMachineStore = args.NewValue as IStore<DevToolsState>;
            if(newTimeMachineStore != null)
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
            _storeSubscription = DevToolsStore.Subscribe(OnStateChange);
        }

        private void OnStateChange(DevToolsState state)
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
            if (DevToolsStore == null 
                || ActionPositionsSlider.Value == _lastState.Position)
            {
                return;
            }

            DevToolsStore.Dispatch(new SetTimeMachinePositionAction
            {
                Position = (int)ActionPositionsSlider.Value
            });
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (DevToolsStore == null)
                return;

            DevToolsStore.Dispatch(new ResumeTimeMachineAction());
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (DevToolsStore == null)
                return;

            DevToolsStore.Dispatch(new PauseTimeMachineAction());
        }
    }
}
