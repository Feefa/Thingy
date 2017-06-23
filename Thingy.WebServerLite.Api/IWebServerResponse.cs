using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public interface IWebServerResponse
    {
        void FromFile(string filePath);
        void NotFound(IWebServerRequest request);
        void NotAllowed(IWebServerRequest request);
        void InternalError(IWebServerRequest request, Exception e);
        void FromString(string content, string contentType);
    }
}
