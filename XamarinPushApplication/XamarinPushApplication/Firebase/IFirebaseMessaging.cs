using System.Threading.Tasks;

namespace XamarinPushApplication.Firebase
{
    public interface IFirebaseMessaging
    {
        Task<string> GetToken();
    }
}
