using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Redux.DevTools.Universal
{
    public sealed class DevFrame : Frame
    {
        public IStore<TimeMachineState> TimeMachineStore
        {
            get { return (IStore<TimeMachineState>)GetValue(TimeMachineStoreProperty); }
            set { SetValue(TimeMachineStoreProperty, value); }
        }
        
        public static readonly DependencyProperty TimeMachineStoreProperty =
            DependencyProperty.Register("TimeMachineStore", typeof(IStore<TimeMachineState>), typeof(DevFrame), new PropertyMetadata(default(IStore<TimeMachineState>)));
        
        public DevFrame()
        {
            this.DefaultStyleKey = typeof(DevFrame);
        }
    }
}
