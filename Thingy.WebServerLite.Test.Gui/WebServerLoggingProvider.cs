using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite.Test.Gui
{
    public partial class WebServerLoggingProvider : Form, IWebServerLoggingProvider
    {
        delegate void LogWriterDelegate(string message);
        private const string logFormatString = "{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13}"; // TODO - Make this customisable


        public WebServerLoggingProvider()
        {
            InitializeComponent();
            Show();
            LogHeaders();

        }

        private void LogHeaders()
        {
            WriteMessage("#Software: Web Server Lite");
            WriteMessage(string.Format("#Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));              
            WriteMessage(string.Format("#Fields: {0}", string.Format(logFormatString,
                "date",
                "time",
                "s-sitename",
                "s-ip",
                "cs-method",
                "cs-uri-stem",
                "cs-uri-query",
                "s-port",
                "cs-username",
                "c-ip",
                "cs(User-Agent)",
                "sc-status",
                "sc-substatus",
                "sc-win32-status")));
        }

        public void LogRequest(IWebServerRequest request, IWebServerResponse response)
        {
            WriteMessage(string.Format(logFormatString,
                DateTime.Now.ToString("yyyy-MM-dd"), //date {0}
                DateTime.Now.ToString("HH:mm:ss"), //time {1}
                PlaceHolderIfBlank(request.WebSite.Name), // s-sitename {2}
                PlaceHolderIfBlank(request.HttpListenerRequest.LocalEndPoint.Address.ToString()), // s-ip {3}
                PlaceHolderIfBlank(request.HttpMethod), // cs-method {4}
                PlaceHolderIfBlank(request.HttpListenerRequest.Url.AbsolutePath), // cs-uri {5}
                PlaceHolderIfBlank(request.HttpListenerRequest.Url.Query), // cs-uri-query {6}
                request.HttpListenerRequest.LocalEndPoint.Port, // s-port {7}
                PlaceHolderIfBlank(request.User.UserId), // cs-username {8}
                PlaceHolderIfBlank(request.HttpListenerRequest.RemoteEndPoint.Address.ToString()), // c-ip {9}
                request.HttpListenerRequest.UserAgent ?? "-", // cs(User-Agent) {10}
                response.HttpListenerResponse.StatusCode, // sc-status {11}
                0, // sc-substatus {12}
                0  // sc-win32-status {13}
                ));
        }

        private object PlaceHolderIfBlank(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "-";
            }

            return value;
        }

        public void WriteMessage(string message)
        {
            if (InvokeRequired)
            {
                LogWriterDelegate d = new LogWriterDelegate(WriteMessage);
                Invoke(d, message);
            }
            else
            {
                MessagesTextBox.Text += message + Environment.NewLine;
            }
        }
    }
}
