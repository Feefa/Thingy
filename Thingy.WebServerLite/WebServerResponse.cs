using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class WebServerResponse : IWebServerResponse
    {
        private readonly HttpListenerResponse response;

        public WebServerResponse(HttpListenerResponse response)
        {
            this.response = response;
        }
    }
}
