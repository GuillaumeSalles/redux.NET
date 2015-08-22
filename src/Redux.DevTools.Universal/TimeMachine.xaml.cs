using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.UI.Xaml.Controls.Primitives;
using Newtonsoft.Json;

namespace Redux.DevTools.Universal
{
    public sealed partial class TimeMachine : UserControl
    {
        private IStore<TimeMachineState> _store;
        private TimeMachineState _lastState;
        
        public TimeMachine()
        {
            this.InitializeComponent();

            _store = (IStore<TimeMachineState>)Application.Current.Resources["TimeMachineStore"];

            _store.Subscribe(state =>
            {
                _lastState = state;

                SignalPositionsSlider.Value = state.Position;
                SignalPositionsSlider.Maximum = state.Signals.Count;

                if(state.Position <= 0)
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
            });
        }

        private void SignalPositionsSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(SignalPositionsSlider.Value == _lastState.Position)
            {
                return;
            }

            _store.Dispatch(new SetTimeMachinePositionSignal { Position = (int)SignalPositionsSlider.Value });
        }
    }
}
