using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public interface IController
    {
        Priorities Priority { get; }

        void Handle(IWebServerRequest request, IWebServerResponse response);

        bool CanHandle(IWebServerRequest request);
    }
}
