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

        public WebServerLoggingProvider()
        {
            InitializeComponent();
            Show();
        }

        public void LogRequest(IWebServerRequest request, IWebServerResponse response)
        {
            WriteMessage(string.Format("{0} {1} {2} {3} {4}",
                request.User.UserId,
                DateTime.Now.Date,
                DateTime.Now.TimeOfDay,
                request.WebSite,
                request.HttpMethod
                ));
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
