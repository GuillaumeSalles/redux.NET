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
            if(oldTimeMachineStore != null)
            {
                timeMachine._storeSubscription.Dispose();
            }

            var newTimeMachineStore = args.NewValue as IStore<TimeMachineState>;
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
            _storeSubscription = TimeMachineStore.Subscribe(OnStateChange);
        }

        private void OnStateChange(TimeMachineState state)
        {
            _lastState = state;

            SignalPositionsSlider.Value = state.Position;
            SignalPositionsSlider.Maximum = state.Signals.Count;

            if (state.Position <= 0)
            {
                CurrentSignalTypeTextBlock.Text = string.Empty;
                CurrentSignalDescription.Text = string.Empty;
            }
            else
            {
                var currentSignal = state.Signals[state.Position - 1];
                CurrentSignalTypeTextBlock.Text = currentSignal.GetType().Name;
                CurrentSignalDescription.Text = JsonConvert.SerializeObject(currentSignal, Formatting.Indented);
            }
        }

        private void SignalPositionsSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (TimeMachineStore == null 
                || SignalPositionsSlider.Value == _lastState.Position)
            {
                return;
            }

            TimeMachineStore.Dispatch(new SetTimeMachinePositionSignal
            {
                Position = (int)SignalPositionsSlider.Value
            });
        }
    }
}
