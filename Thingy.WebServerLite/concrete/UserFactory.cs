using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class UserFactory : IUserFactory
    {
        public IUser CreateFailedUser()
        {
            return new User("Failed");
        }

        public IUser CreateGuestUser()
        {
            return new User("Guest");
        }

        public IUser CreateUserFromKnownUser(IKnownUser knownUser)
        {
            return new User(knownUser.UserId, knownUser.Roles);
        }
    }
}
