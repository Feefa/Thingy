using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public interface IUserProvider
    {
        IUser GetUser(string ipAddress, string userId, string password);
    }
}