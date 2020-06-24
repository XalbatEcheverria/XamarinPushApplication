using System.Threading.Tasks;

namespace XamarinPushApplication.Interfaces
{
    public interface IFirebaseMessaging
    {
        Task<string> GetToken();
    }
}
