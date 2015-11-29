using Android.App;
using Android.OS;
using Android.Content.PM;

namespace Redux.TodoMvc.Forms.Android
{
    [Activity(
        Label = "Redux.TodoMvc.Forms.Android", 
        Icon = "@drawable/icon", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }
}

