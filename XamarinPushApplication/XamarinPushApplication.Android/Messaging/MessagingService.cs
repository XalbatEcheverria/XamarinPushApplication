using System;
using System.Collections.Generic;
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
    public class MessagingService : FirebaseMessagingService
    {
        const string TAG = "MessagingService";

        public bool MessagePending { get; set; }
        public IDictionary<string, string> MessageData { get; set; }

        private readonly IMessageManager _messageManager;

        public MessagingService() : base() 
        {
            _messageManager = InjectionContainer.IoCContainer.GetInstance<IMessageManager>();
        }

        public MessagingService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            _messageManager = InjectionContainer.IoCContainer.GetInstance<IMessageManager>();
        }

        public override void OnMessageReceived(RemoteMessage p0)
        {
            string title = p0.Data["Title"];
            p0.Data.Remove("Title");
            string message = p0.Data["Message"];
            p0.Data.Remove("Message");

            _messageManager.ScheduleNotification(title, message, p0.Data);
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