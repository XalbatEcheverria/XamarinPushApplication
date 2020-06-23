using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Iid;
using Firebase.Messaging;

namespace XamarinPushApplication.Droid.Messaging
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseIIDService";
        int id = 0;

        public MyFirebaseMessagingService() : base() { }
        public MyFirebaseMessagingService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public override void OnMessageReceived(RemoteMessage p0)
        {
            var intent = new Intent(this, typeof(MainActivity));
            var pending = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            string channelId = "default";
            var builder = new NotificationCompat.Builder(this, channelId)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetColor(Resource.Color.notification_icon_bg_color)
                .SetContentTitle(p0.Data["Title"])
                .SetContentText(p0.Data["Message"])
                .SetAutoCancel(true)
                .SetContentIntent(pending)
                .AddAction(new NotificationCompat.Action(Resource.Drawable.icon, "Accept", pending))
                .AddAction(new NotificationCompat.Action(Resource.Drawable.icon, "Decline", pending));

            var manager = (NotificationManager)GetSystemService(NotificationService);
            if(Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(channelId, "Default channel", NotificationImportance.High);
                manager.CreateNotificationChannel(channel);
            }
            manager.Notify(id, builder.Build());
            id++;
        }

        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            Log.Debug(TAG, $"New token: {p0}");
            //TODO | Send registration to server
        }

        public async static Task<string> GetToken()
        {
            var instanceIdResult = await FirebaseInstanceId.Instance.GetInstanceId().AsAsync<IInstanceIdResult>();
            return instanceIdResult.Token;
        }
    }
}