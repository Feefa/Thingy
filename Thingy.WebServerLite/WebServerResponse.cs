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
            HttpListenerResponse.StatusCode = 200;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            using (Stream outputStream = HttpListenerResponse.OutputStream)
            {
                fileStream.CopyTo(HttpListenerResponse.OutputStream);
                fileStream.Close();
                outputStream.Close();
            }
        }

        public void FromString(string content, string contentType, int statusCode)
        {
            HttpListenerResponse.ContentType = contentType;
            HttpListenerResponse.StatusCode = statusCode;

            using (Stream outputStream = HttpListenerResponse.OutputStream)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                HttpListenerResponse.ContentLength64 = buffer.Length;
                outputStream.Write(buffer, 0, buffer.Length);
                outputStream.Close();
            }
        }

        public void InternalError(IWebServerRequest request, Exception e)
        {
            FromString(new MarkUpBuilder()
                .Append("<!DOCTYPE html>", "<html>", "<head>", "<title>500 Error</title>", "</head>")
                .Append("<body style=\"font-family: calibri, ariel;\">", "<h1 style=\"background-color: #ffffcf; border : 1px solid black; padding : 8px\">500 - Internal Server Error</h1>")
                .Append("<div style=\"font-family: consolas, courier; border : 1px solid black; padding : 8px\">")
                .Append(e.ToString().Replace("\n", "<br/>"))
                .Append("</div></body>", "</html>")
                .ToString(), "text/html", 500);
        }

        public void NotAllowed(IWebServerRequest request)
        {
            FromString(new MarkUpBuilder()
                .Append("<!DOCTYPE html>", "<html>", "<head>", "<title>403 Forbidden</title>", "</head>")
                .Append("<body style=\"font-family: calibri, ariel;\">", "<h1 style=\"background-color: #ffffcf; border : 1px solid black; padding : 8px\">500 - Forbidden</h1>")
                .Append("<div style=\"font-family: consolas, courier; border : 1px solid black; padding : 8px\">")
                .Append("The request was valid, but the server is refusing action.<br/>The user might not have the necessary permissions for a resource, or may need an account of some sort.")
                .Append("</div></body>", "</html>")
                .ToString(), "text/html", 403);
        }

        public void NotFound(IWebServerRequest request)
        {
            FromString(new MarkUpBuilder()
                .Append("<!DOCTYPE html>", "<html>", "<head>", "<title>403 Not Found</title>", "</head>")
                .Append("<body style=\"font-family: calibri, ariel;\">", "<h1 style=\"background-color: #ffffcf; border : 1px solid black; padding : 8px\">404 - Not Found</h1>")
                .Append("<div style=\"font-family: consolas, courier; border : 1px solid black; padding : 8px\">")
                .Append("The requested resource could not be found but may be available in the future.<br/>Subsequent requests by the client are permissible.")
                .Append("</div></body>", "</html>")
                .ToString(), "text/html", 404);
        }

        public HttpListenerResponse HttpListenerResponse { get; private set; }
    }
}
