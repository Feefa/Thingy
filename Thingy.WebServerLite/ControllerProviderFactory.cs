using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class ControllerProviderFactory : IControllerProviderFactory
    {
        public IControllerProvider Create(IController[] controllers)
        {
            return new ControllerProvider(controllers);
        }
    }
}
