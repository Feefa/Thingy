using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class WebServerResponse : IWebServerResponse
    {
        private readonly IMimeTypeProvider mimeTypeProvider;

        public WebServerResponse(IMimeTypeProvider mimeTypeProvider, HttpListenerResponse response)
        {
            this.mimeTypeProvider = mimeTypeProvider;
            this.HttpListenerResponse = response;
        }

        public void FromFile(string filePath)
        {
            HttpListenerResponse.ContentType = mimeTypeProvider.GetMimeType(filePath);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            using (Stream outputStream = HttpListenerResponse.OutputStream)
            {
                fileStream.CopyTo(HttpListenerResponse.OutputStream);
                fileStream.Close();
                outputStream.Close();
            }

            HttpListenerResponse.Close();
        }

        public void FromString(string content, string contentType)
        {
            HttpListenerResponse.ContentType = contentType;

            using (Stream outputStream = HttpListenerResponse.OutputStream)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                HttpListenerResponse.ContentLength64 = buffer.Length;
                outputStream.Write(buffer, 0, buffer.Length);
                outputStream.Close();
            }

            HttpListenerResponse.Close();
        }

        public void InternalError(IWebServerRequest request, Exception e)
        {
            FromString(string.Format("<html><head><title>500 Error</title></head><body><h1>500 - Internal Server Error</h2></h2><p>{0}</p></body></html>", e.ToString()), "text/html");
        }

        public void NotAllowed(IWebServerRequest request)
        {
            FromString("<html><head><title>403 Forbidden</title></head><body><h1>403 - Forbidden</h2></h2><p>The request was valid, but the server is refusing action. The user might not have the necessary permissions for a resource, or may need an account of some sort.</p></body></html>", "text/html");
        }

        public void NotFound(IWebServerRequest request)
        {
            FromString("<html><head><title>404 Not Found</title></head><body><h1>404 - Not Found</h2></h2><p>The requested resource could not be found but may be available in the future. Subsequent requests by the client are permissible.</p></body></html>", "text/html");
        }

        public HttpListenerResponse HttpListenerResponse { get; private set; }
    }
}
