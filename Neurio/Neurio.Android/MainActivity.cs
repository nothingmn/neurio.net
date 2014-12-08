using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Neurio.Client;
using Xamarin.Forms.Platform.Android;

namespace Neurio.Droid
{
    [Activity(Label = "Neurio", MainLauncher = true)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            var client = new NeurioClient();

            SetPage(App.GetMainPage(client));
        }
    }
}

