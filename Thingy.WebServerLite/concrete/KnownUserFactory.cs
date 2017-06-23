using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite
{
    public class KnownUserFactory : IKnownUserFactory
    {
        private readonly string[] defaultRoles;
        private readonly IKnownUser[] defaultKnownUsers;
        private readonly char[] comma = new char[] { ',' };
        private readonly char[] semiColon = new char[] { ',' };

        public KnownUserFactory(string[] defaultRoles, string[] users)
        {
            this.defaultRoles = defaultRoles;
            this.defaultKnownUsers = users.Select(u => u.Split(comma)).Select(s => Create("", s[0], s[1], s[2].Split(semiColon))).ToArray();
        }

        public IKnownUser Create(string ipAddress, string userId, string password)
        {
            return Create(ipAddress, userId, password, defaultRoles);
        }

        public IKnownUser Create(string ipAddress, string userId, string password, string[] roles)
        {
            return new KnownUser(ipAddress, userId, password, roles);
        }

        public IEnumerable<IKnownUser> GetDefaultKnownUsers()
        {
            return defaultKnownUsers;
        }
    }
}
