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
    public partial class MainForm : Form, IMainForm
    {
        private IWebServer webServer;

        public MainForm(IWebServer webServer)
        {
            InitializeComponent();

            this.webServer = webServer;
        }

        public Form Form
        {
            get
            {
                return this;
            }
        }

        private void MessagesTextBox_DoubleClick(object sender, EventArgs e)
        {
            if (webServer.IsStarted)
            {
                webServer.Stop();
            }
            else
            {
                webServer.Start();
            }
        }
    }
}
