using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Tasks;
using Android.OS;
using System;
using System.Threading.Tasks;
using XamarinPushApplication.Firebase;

namespace XamarinPushApplication.Droid.Messaging
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class AndroidMessaging : IFirebaseMessaging
    {
        internal static readonly string CHANNEL_ID = "userlock_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        public AndroidMessaging(Context context)
        {
            CheckPlayServicesAvailable(context);
            CreateNotificationChannel(context);
        }

        public static void CheckPlayServicesAvailable(Context context)
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
            if(resultCode != ConnectionResult.Success)
            {
                string error;
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    error = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    error = "This device is not supported";
                }
                throw new ApplicationException(error);
            }
        }

        private void CreateNotificationChannel(Context context)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications",  NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public async Task<string> GetToken()
        {
            return await MyFirebaseMessagingService.GetToken();
        }
    }
}