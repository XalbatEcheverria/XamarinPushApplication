using Android.App;
using Android.Content;

namespace XamarinPushApplication.Droid.Messaging
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { "com.companyname.xamarinpushnotifications.MFA" })]
    public class MFABroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            int notificationId = intent.GetIntExtra("notificationId", 0);
            var manager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            manager.Cancel(notificationId);
        }
    }
}