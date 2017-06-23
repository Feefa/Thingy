using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public interface IUserFactory
    {
        IUser CreateGuestUser();
        IUser CreateFailedUser();
        IUser CreateUserFromKnownUser(IKnownUser knownUser);
    }
}