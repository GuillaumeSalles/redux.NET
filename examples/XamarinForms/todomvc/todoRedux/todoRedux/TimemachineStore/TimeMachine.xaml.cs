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
            _lastState = state;

            Shield.IsVisible = state.IsPaused ? true : false;

			PauseButton.IsVisible = state.IsPaused ? false : true;
			PlayButton.IsVisible = state.IsPaused ? true : false;

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
				CurrentActionDescription.Text = currentAction.ToString ();
//                CurrentActionDescription.Text = JsonConvert.SerializeObject(currentAction, Formatting.Indented);
            }
        }

        private void ActionPositionsSlider_ValueChanged(double oldvalue, double newvalue)
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
