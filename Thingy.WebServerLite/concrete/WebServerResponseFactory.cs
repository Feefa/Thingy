using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class WebServerResponseFactory : IWebServerResponseFactory
    {
        private IMimeTypeProvider mimeTypeProvider;

        public WebServerResponseFactory(IMimeTypeProvider mimeTypeProvider)
        {
            this.mimeTypeProvider = mimeTypeProvider;
        }

        public IWebServerResponse Create(HttpListenerResponse response)
        {
            return new WebServerResponse(mimeTypeProvider, response);
        }
    }
}
