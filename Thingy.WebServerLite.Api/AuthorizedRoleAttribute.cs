using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizedRoleAttribute : Attribute
    {
        public AuthorizedRoleAttribute(string role)
        {
            Role = role;
        }

        public string Role { get; private set; }
    }
}
