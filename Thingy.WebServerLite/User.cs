using System;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    internal class User : IUser
    {
        public User(string userId)
        {
            UserId = userId;
            Roles = new string[] { };
        }

        public User(string userId, string[] roles)
        {
            UserId = userId;
            Roles = roles;
        }

        public string[] Roles { get; private set; }

        public string UserId { get; private set; }
    }
}