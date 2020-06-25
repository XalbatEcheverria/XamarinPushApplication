﻿using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using XamarinPushApplication.Interfaces;

namespace XamarinPushApplication.Droid.Messaging
{
    public class MessageManager : IMessageManager
    {
        private readonly object _dataLock = new { };
        private readonly object _statusLock = new { };
        private readonly object _idLock = new { };

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

            MessagePending = true;
            MessageData = additionalData;
            NotificationId = notificationId;

            return notificationId;
        }

        public void DeleteNotification(int? notificationId)
        {
            if(notificationId != null)
            {
                var manager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
                manager.Cancel((int)notificationId);
            }
        }
    }
}