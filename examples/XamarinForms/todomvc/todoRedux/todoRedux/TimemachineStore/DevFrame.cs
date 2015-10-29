using Xamarin.Forms;
using Redux;

namespace todoRedux
{
	public sealed class DevFrame : ContentPage
    {
        public DevFrame(IStore<TimeMachineState> store)
        {
            var timeMachineView = new TimeMachine();
            timeMachineView.TimeMachineStore = store;
            Content = timeMachineView;
            BackgroundColor = Color.FromRgb(245, 245, 245);
        }
    }
}
