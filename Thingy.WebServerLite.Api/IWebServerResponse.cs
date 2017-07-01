using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public interface IWebServerResponse
    {
        HttpListenerResponse HttpListenerResponse { get; }

        void FromFile(string filePath);
        void NotFound(IWebServerRequest request);
        void NotAllowed(IWebServerRequest request);
        void InternalError(IWebServerRequest request, Exception e);
        void FromString(string content, string contentType, int statusCode = 200);
    }
}
