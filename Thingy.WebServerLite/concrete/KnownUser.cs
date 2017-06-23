using System;

namespace Thingy.WebServerLite
{
    internal class KnownUser : IKnownUser
    {
        public KnownUser(string ipAddress, string userId, string password, string[] roles)
        {
            IpAddress = ipAddress;
            UserId = userId;
            Password = password;
            Roles = roles;
        }

        public string IpAddress { get; set; }

        public string Password { get; private set; }

        public string[] Roles { get; private set; }

        public string UserId { get; private set; }
    }
}