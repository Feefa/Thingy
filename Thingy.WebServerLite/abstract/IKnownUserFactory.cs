using System.Collections.Generic;

namespace Thingy.WebServerLite
{
    public interface IKnownUserFactory
    {
        IEnumerable<IKnownUser> GetDefaultKnownUsers();
        IKnownUser Create(string ipAddress, string userId, string password);
    }
}