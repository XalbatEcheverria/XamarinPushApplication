namespace XamarinPushApplication.Interfaces
{
    public interface IMessagingService
    {
        int ScheduleNotification(string title, string message);
    }
}
