using System.Threading.Tasks;
using XamarinPushApplication.Interfaces;

namespace XamarinPushApplication.Droid.Messaging
{
    public class TokenAccessor : ITokenAccessor
    {
        public async Task<string> GetToken()
        {
            return await MessagingService.GetToken();
        }
    }
}