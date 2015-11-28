using System;
using Xamarin.Forms;
using Redux;

namespace todoRedux
{
	public partial class TimeMachine : ContentView
    {
        private IDisposable _storeSubscription;
        private TimeMachineState _lastState;

        public IStore<TimeMachineState> TimeMachineStore
        {
            get { return (IStore<TimeMachineState>)GetValue(TimeMachineStoreProperty); }
            set { SetValue(TimeMachineStoreProperty, value); }
        }

		public static readonly BindableProperty TimeMachineStoreProperty =
			BindableProperty.Create<TimeMachine, IStore<TimeMachineState>>(p=>p.TimeMachineStore,null,BindingMode.Default,null,OnTimeMachineStoreChanged);
        
		private static void OnTimeMachineStoreChanged(BindableObject obj, IStore<TimeMachineState> oldValue, IStore<TimeMachineState> newValue)
        {
			var timeMachine = (TimeMachine)obj;

			if(oldValue != null)
            {
                timeMachine._storeSubscription.Dispose();
            }
		
			if(newValue != null)
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
            _canLetValueChange = false;

            _lastState = state;

            Shield.IsVisible = state.IsPaused ? true : false;

            PauseButton.IsEnabled = state.IsPaused ? false : true;

            PlayButton.IsEnabled = state.IsPaused ? true : false;

            counterLabel.Text = string.Format ("Position: {0}, Actions: {1}", state.Position, state.Actions.Count);

            if (ActionPositionsStepper.Value != state.Position)
                ActionPositionsStepper.Value = state.Position;

            if (ActionPositionsStepper.Maximum != state.Actions.Count && state.Actions.Count > 0)
                ActionPositionsStepper.Maximum = state.Actions.Count;
            
            if (state.Position <= 0)
            {
                CurrentActionTypeTextBlock.Text = string.Empty;
                CurrentActionDescription.Text = string.Empty;
            }
            else
            {
                var currentAction = state.Actions[state.Position - 1];
                CurrentActionTypeTextBlock.Text = currentAction.GetType().Name;
				CurrentActionDescription.Text = currentAction.ToString ();
//                CurrentActionDescription.Text = JsonConvert.SerializeObject(currentAction, Formatting.Indented);
            }
            _canLetValueChange = true;
        }

        bool _canLetValueChange = false;

        private void ActionPositionsStepper_ValueChanged(object sender, EventArgs e)
        {
            if (_canLetValueChange) 
            {
                _canLetValueChange = false;

                if (TimeMachineStore == null
                    || ActionPositionsStepper.Value == _lastState.Position) {
                    return;
                }

                TimeMachineStore.Dispatch (new SetTimeMachinePositionAction {
                    Position = (int)ActionPositionsStepper.Value
                });
            }
        }

		private void PlayButton_Click(object sender, EventArgs e)
        {
            if (TimeMachineStore == null)
                return;

            TimeMachineStore.Dispatch(new ResumeTimeMachineAction());
        }

		private void PauseButton_Click(object sender, EventArgs e)
        {
            if (TimeMachineStore == null)
                return;

            TimeMachineStore.Dispatch(new PauseTimeMachineAction());
        }
    }
}
