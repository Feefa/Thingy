using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public interface IUser
    {
        ////string IpAddress { get; set; }
        ////string Password { get; set; }
        string[] Roles { get; }

        string UserId { get; }
    }
}
