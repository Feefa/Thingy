using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class WebServerResponse : IWebServerResponse
    {
        private readonly IMimeTypeProvider mimeTypeProvider;

        private Mutex fileIoMutex = new Mutex(false, "04f0fd63-10a5-4c01-a4be-50d10021d6d5");

        public WebServerResponse(IMimeTypeProvider mimeTypeProvider, HttpListenerResponse response)
        {
            this.mimeTypeProvider = mimeTypeProvider;
            this.HttpListenerResponse = response;
        }

        public void FromFile(string filePath)
        {
            HttpListenerResponse.ContentType = mimeTypeProvider.GetMimeType(filePath);
            HttpListenerResponse.StatusCode = 200;

            try
            {
                using (Stream outputStream = HttpListenerResponse.OutputStream)
                {
                    fileIoMutex.WaitOne();

                    try
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            fileStream.CopyTo(HttpListenerResponse.OutputStream);
                            fileStream.Close();
                        }
                    }
                    finally
                    {
                        fileIoMutex.ReleaseMutex();
                    }

                    outputStream.Close();
                }
            }
            catch(Exception exception)
            {
                InternalError(null, exception);
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
            FromString(
                AddRequestDetails(new MarkUpBuilder()
                .Append("<!DOCTYPE html>", "<html>", "<head>", "<title>500 Error</title>", "</head>")
                .Append("<body style=\"font-family: calibri, ariel;\">", "<h1 style=\"background-color: #ffffcf; border : 1px solid black; padding : 8px\">500 - Internal Server Error</h1>")
                , request)
                .Append("<div style=\"font-family: consolas, courier; border : 1px solid black; padding : 8px\">")
                .Append(e.ToString().Replace("\n", "<br/>"))
                .Append("</div></body>", "</html>")
                .ToString(), "text/html", 500);
        }

        public void NotAllowed(IWebServerRequest request)
        {
            FromString(
                AddRequestDetails(new MarkUpBuilder()
                .Append("<!DOCTYPE html>", "<html>", "<head>", "<title>403 Forbidden</title>", "</head>")
                .Append("<body style=\"font-family: calibri, ariel;\">", "<h1 style=\"background-color: #ffffcf; border : 1px solid black; padding : 8px\">500 - Forbidden</h1>")
                , request)
                .Append("<div style=\"font-family: consolas, courier; border : 1px solid black; padding : 8px\">")
                .Append("The request was valid, but the server is refusing action.<br/>The user might not have the necessary permissions for a resource, or may need an account of some sort.")
                .Append("</div></body>", "</html>")
                .ToString(), "text/html", 403);
        }

        public void NotFound(IWebServerRequest request)
        {
            FromString(
                AddRequestDetails(new MarkUpBuilder()
                .Append("<!DOCTYPE html>", "<html>", "<head>", "<title>404 Not Found</title>", "</head>")
                .Append("<body style=\"font-family: calibri, ariel;\">", "<h1 style=\"background-color: #ffffcf; border : 1px solid black; padding : 8px\">404 - Not Found</h1>")
                , request)
                .Append("<div style=\"font-family: consolas, courier; border : 1px solid black; padding : 8px\">")
                .Append("The requested resource could not be found but may be available in the future.<br/>Subsequent requests by the client are permissible.")
                .Append("</div></body>", "</html>")
                .ToString(), "text/html", 404);
        }

        private MarkUpBuilder AddRequestDetails(MarkUpBuilder builder, IWebServerRequest request)
        {
            return request == null ? builder : builder
                .Append("<div style=\"font-family: consolas, courier; border : 1px solid black; padding : 8px; margin-bottom : 4px;\">")
                .Append("<table><tbody>")
                .Append("<tr><th style=\"padding-right : 4px; text-align : right\">HTTP Method</th><td>", request.HttpMethod, "</td></tr>")
                .Append("<tr><th style=\"padding-right : 4px; text-align : right\">Url</th><td>", request.HttpListenerRequest.Url, "</td></tr>")
                .Append("</tbody></table>")
                .Append("</div>");
        }

        public HttpListenerResponse HttpListenerResponse { get; private set; }
    }
}
