using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using XamarinPushApplication.Interfaces;
using static Android.App.ActivityManager;

namespace XamarinPushApplication.Droid.Messaging
{
    public class MessageManager : IMessageManager
    {
        private readonly object _statusLock = new { };
        private bool messagePending = false;
        public bool MessagePending { 
            get{
                return messagePending;
            }
            set{
                lock (_statusLock)
                {
                    messagePending = value;
                }
            }}

        private readonly object _dataLock = new { };
        private IDictionary<string, string> messageData;
        public IDictionary<string, string> MessageData
        {
            get
            {
                return messageData;
            }
            set
            {
                lock (_dataLock)
                {
                    messageData = value;
                }
            }
        }

        private readonly object _idLock = new { };
        private int? notificationId;
        public int? NotificationId
        {
            get
            {
                return notificationId;
            }
            set
            {
                lock (_idLock)
                {
                    notificationId = value;
                }
            }
        }

        private readonly object _titleLock = new { };
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                lock (_titleLock)
                {
                    _title = value;
                }
            }
        }

        private readonly object _messageLock = new { };
        private string _message;
        public string Target
        {
            get
            {
                return _message;
            }
            set
            {
                lock (_messageLock)
                {
                    _message = value;
                }
            }
        }


        public int ScheduleNotification(string title, string message, IDictionary<string, string> additionalData)
        {
            int notificationId = 0;
            var acceptationIntent = new Intent("com.companyname.xamarinpushnotifications.MFA").PutExtra("action", "Accept").PutExtra("notificationId", notificationId);
            var denialIntent = new Intent("com.companyname.xamarinpushnotifications.MFA").PutExtra("action", "Deny").PutExtra("notificationId", notificationId);
            var contentIntent = new Intent(Application.Context, typeof(MainActivity)).PutExtra("notificationId", notificationId);

            var pendingAccept = PendingIntent.GetBroadcast(Application.Context, 0, acceptationIntent, PendingIntentFlags.UpdateCurrent);
            var pendingDeny = PendingIntent.GetBroadcast(Application.Context, 1, denialIntent, PendingIntentFlags.UpdateCurrent);
            var pendingContent = PendingIntent.GetActivity(Application.Context, 2, contentIntent, PendingIntentFlags.UpdateCurrent);

            var builder = new NotificationCompat.Builder(Application.Context, MessagingInitializer.CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetColor(Resource.Color.launcher_background)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetAutoCancel(false)
                .SetContentIntent(pendingContent)
                .AddAction(Resource.Drawable.icon, "Accept", pendingAccept)
                .AddAction(Resource.Drawable.icon, "Decline", pendingDeny);

            var manager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(MessagingInitializer.CHANNEL_ID, "Default channel", NotificationImportance.High);
                manager.CreateNotificationChannel(channel);
            }
            manager.Notify(notificationId, builder.Build());

            Title = title;
            Target = message;
            MessagePending = true;
            MessageData = additionalData;
            NotificationId = notificationId;

            if (IsApplicationForeground())
                Application.Context.StartActivity(contentIntent);

            return notificationId;
        }

        public void DeleteMessage(int? notificationId)
        {
            if(notificationId != null)
            {
                var manager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
                manager.Cancel((int)notificationId);

                Title = null;
                Target = null;
                MessagePending = false;
                MessageData = null;
                NotificationId = null;
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