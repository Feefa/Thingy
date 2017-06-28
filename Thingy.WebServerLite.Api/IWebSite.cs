using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public interface IWebSite
    {
        int PortNumber { get; }

        Priorities Priority { get; }

        bool CanHandle(IWebServerRequest request);

        void Handle(IWebServerRequest request, IWebServerResponse response);

        string GetOsFilePath(string fileName);

        IViewProvider ViewProvider { get; }

        bool IsDefault { get; }

        string Name { get; }
    }
}
