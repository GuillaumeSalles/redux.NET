using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace Redux.TodoMvc.Android
{
    [Activity(Label = "Redux.TodoMvc.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);



            // Get our button from the layout resource,
            // and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.MyButton);

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
             

            //var newFragment = new Header();
            //var ft = FragmentManager.BeginTransaction();
            //ft.Add(Resource.Layout.Main, newFragment);
            //ft.Commit();


        }

        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            return base.OnCreateView(parent, name, context, attrs);
        }
    }
}

