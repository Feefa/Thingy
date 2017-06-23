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

        public void FromFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void FromString(string content, string contentType)
        {
            throw new NotImplementedException();
        }

        public void InternalError(IWebServerRequest request, Exception e)
        {
            throw new NotImplementedException();
        }

        public void NotAllowed(IWebServerRequest request)
        {
            throw new NotImplementedException();
        }

        public void NotFound(IWebServerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
