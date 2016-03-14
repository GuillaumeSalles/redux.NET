using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Redux.DevTools.Universal
{
    public sealed class DevFrame : Frame
    {
        public IStore<DevToolsState> DevToolsStore
        {
            get { return (IStore<DevToolsState>)GetValue(DevToolsStoreProperty); }
            set { SetValue(DevToolsStoreProperty, value); }
        }
        
        public static readonly DependencyProperty DevToolsStoreProperty =
            DependencyProperty.Register("DevToolsStore", typeof(IStore<DevToolsState>), typeof(DevFrame), new PropertyMetadata(default(IStore<DevToolsState>)));
        
        public DevFrame()
        {
            this.DefaultStyleKey = typeof(DevFrame);
        }
    }
}
