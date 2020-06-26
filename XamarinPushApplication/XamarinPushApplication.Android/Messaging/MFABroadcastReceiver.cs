using Android.App;
using Android.Content;
using XamarinPushApplication.Interfaces;
using static Android.App.ActivityManager;

namespace XamarinPushApplication.Droid.Messaging
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { "com.companyname.xamarinpushnotifications.MFA" })]
    public class MFABroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            int notificationId = intent.GetIntExtra("notificationId", 0);
            var messageManager = InjectionContainer.IoCContainer.GetInstance<IMessageManager>();
            messageManager.DeleteMessage(notificationId);

            if (IsApplicationForeground())
            {
                var contentIntent = new Intent(Application.Context, typeof(MainActivity));
                Application.Context.StartActivity(contentIntent);
            }
        }

        private bool IsApplicationForeground()
        {
            var myProcess = new RunningAppProcessInfo();
            GetMyMemoryState(myProcess);
            return myProcess.Importance == Importance.Foreground;
        }
    }
}