using System.Collections.Generic;

namespace XamarinPushApplication.Interfaces
{
    public interface IMessageManager
    {
        bool MessagePending { get; set; }
        IDictionary<string, string> MessageData { get; set; }
        int? NotificationId { get; set; }

        int ScheduleNotification(string title, string message, IDictionary<string,string> additionalData);
        void DeleteNotification(int? notificationId);
    }
}
