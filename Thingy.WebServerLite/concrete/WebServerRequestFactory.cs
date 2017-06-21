using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class WebServerRequestFactory : IWebServerRequestFactory
    {
        public IWebServerRequest Create(HttpListenerRequest request)
        {
            return new WebServerRequest(request);
        }
    }
}
