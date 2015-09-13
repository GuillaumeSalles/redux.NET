using Android.App;
using Android.OS;
using Android.Views;

namespace Redux.TodoMvc.Android
{
    public class Header : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.Header, container, false);
        }
    }
}