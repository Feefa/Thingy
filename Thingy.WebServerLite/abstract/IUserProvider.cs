using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public interface IUserProvider
    {
        /// <summary>
        /// Get a user given the specified credentials
        /// The user will be created if enough information is provided
        /// with default roles
        /// </summary>
        /// <param name="ipAddress">The IP address that the request came from</param>
        /// <param name="userId">Any user id specified</param>
        /// <param name="password">Any password specified</param>
        /// <returns></returns>
        IUser GetUser(string ipAddress, string userId, string password);
    }
}