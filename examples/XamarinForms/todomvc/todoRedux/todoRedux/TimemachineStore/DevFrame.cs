using Xamarin.Forms;
using Redux;

namespace todoRedux
{
	public sealed class DevFrame : ContentPage
    {
        public IStore<TimeMachineState> TimeMachineStore
        {
            get { return (IStore<TimeMachineState>)GetValue(TimeMachineStoreProperty); }
            set { SetValue(TimeMachineStoreProperty, value); }
        }
        
		public static readonly BindableProperty TimeMachineStoreProperty =
			BindableProperty.Create<DevFrame, IStore<TimeMachineState>>(p => p.TimeMachineStore,null);
        
        public DevFrame()
        {
            //this.DefaultStyleKey = typeof(DevFrame);
        }
    }
}
