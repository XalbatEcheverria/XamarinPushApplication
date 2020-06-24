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
using XamarinPushApplication.Interfaces;

namespace XamarinPushApplication.Droid.Messaging
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MessagingService : FirebaseMessagingService, IMessagingService
    {
        const string TAG = "MessagingService";

        public MessagingService() : base() { }
        public MessagingService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public override void OnMessageReceived(RemoteMessage p0)
        {
            ScheduleNotification(p0.Data["Title"], p0.Data["Message"]);
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

        public int ScheduleNotification(string title, string message)
        {
            int notificationId = DateTime.Now.Millisecond;
            var acceptationIntent = new Intent("com.companyname.xamarinpushnotifications.MFA").PutExtra("action", "Accept").PutExtra("notificationId", notificationId);
            var denialIntent = new Intent("com.companyname.xamarinpushnotifications.MFA").PutExtra("action", "Deny").PutExtra("notificationId", notificationId);

            var pendingAccept = PendingIntent.GetBroadcast(this, 0, acceptationIntent, PendingIntentFlags.UpdateCurrent);
            var pendingDeny = PendingIntent.GetBroadcast(this, 1, denialIntent, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(Application.Context, AndroidMessaging.CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetColor(Resource.Color.launcher_background)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetAutoCancel(false)
                .AddAction(Resource.Drawable.icon, "Accept", pendingAccept)
                .AddAction(Resource.Drawable.icon, "Decline", pendingDeny);

            var manager = (NotificationManager)GetSystemService(NotificationService);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(AndroidMessaging.CHANNEL_ID, "Default channel", NotificationImportance.High);
                manager.CreateNotificationChannel(channel);
            }
            manager.Notify(notificationId, builder.Build());
            return notificationId;
        }
    }
}