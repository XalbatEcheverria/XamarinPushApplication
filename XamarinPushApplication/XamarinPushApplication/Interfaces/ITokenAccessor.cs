using System.Threading.Tasks;

namespace XamarinPushApplication.Interfaces
{
    public interface ITokenAccessor
    {
        Task<string> GetToken();
    }
}
