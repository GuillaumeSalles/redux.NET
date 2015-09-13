using Android.App;
using Android.OS;
using Android.Views;

namespace Redux.TodoMvc.Android
{
    public class Footer : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.Footer, container, false);
        }
    }
}