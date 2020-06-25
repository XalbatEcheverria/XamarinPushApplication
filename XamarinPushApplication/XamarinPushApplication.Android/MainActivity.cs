using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using XamarinPushApplication.Droid.Messaging;
using Android.Util;
using Android.Content;
using XamarinPushApplication.Interfaces;
using SimpleInjector;
using XamarinPushApplication.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinPushApplication.Enums;
using static Android.App.ActivityManager;
using System;

namespace XamarinPushApplication.Droid
{
    [Activity(Label = "XamarinPushApplication", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const string TAG = "XamarinPushApp";
        MainActivity Instance;
        ITokenAccessor _tokenAccessor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);

            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }
            MessagingInitializer.CheckPlayServicesAvailable(this);
            MessagingInitializer.CreateNotificationChannel(this);

            InjectionContainer.IoCContainer.Register<IMessageManager, MessageManager>(Lifestyle.Singleton);

            _tokenAccessor = new TokenAccessor();
            LoadApplication(new App(_tokenAccessor));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        protected override void OnNewIntent(Intent intent)
        {
            LoadApplication(new App(_tokenAccessor, (int)RequestedPage.MFA));
        }
    }
}