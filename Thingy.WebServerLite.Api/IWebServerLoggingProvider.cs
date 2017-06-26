using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public interface IWebServerLoggingProvider
    {
        void WriteMessage(string v);
        void LogRequest(IWebServerRequest request, IWebServerResponse response);
    }
}
