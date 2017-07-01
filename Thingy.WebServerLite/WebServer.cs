using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class WebServer : IWebServer, IDisposable
    {
        private readonly IWebSite[] webSites;
        private readonly IWebServerRequestFactory webServerRequestFactory;
        private readonly IWebServerResponseFactory webServerResponseFactory;
        private readonly IWebServerLoggingProvider logger;

        HttpListener listener = null;

        public WebServer(
            IWebSite[] webSites,
            IWebServerRequestFactory webServerRequestFactory,
            IWebServerResponseFactory webServerResponseFactory,
            IWebServerLoggingProvider logger)
        {
            this.webSites = webSites.OrderBy(w => w.Priority).ToArray();
            this.webServerRequestFactory = webServerRequestFactory;
            this.webServerResponseFactory = webServerResponseFactory;
            this.logger = logger;
        }

        public void Dispose()
        {
            if (listener != null)
            {
                listener.Close();
                listener = null;
            }

            DeleteHttpNameSpaceReservations();
        }

        public void Start()
        {
            if (listener != null)
            {
                if (listener.IsListening)
                {
                    throw new InvalidOperationException("The web server is already started");
                }
            }
            else
            {
                listener = new HttpListener();
            }

            foreach(IWebSite webSite in webSites)
            {
                string prefix = string.Format("http://*:{0}/", webSite.PortNumber);
                listener.Prefixes.Add(prefix);
                AddHttpNameSpaceReservation(prefix);
            }

            listener.Start();
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
            logger.WriteMessage("Web server started");
        }

        private void DeleteHttpNameSpaceReservations()
        {
            if (webSites != null)
            {
                foreach (IWebSite webSite in webSites)
                {
                    string prefix = string.Format("http://*:{0}/", webSite.PortNumber);
                    DeleteHttpNameSpaceReservation(prefix);
                }
            }
        }

        private void AddHttpNameSpaceReservation(string prefix)
        {
            RunProcess(string.Format("netsh http add urlacl url={0} user={1}", prefix, WindowsIdentity.GetCurrent().Name));
        }

        private void DeleteHttpNameSpaceReservation(string prefix)
        {
            RunProcess(string.Format("netsh http delete urlacl url={0}", prefix));
        }

        private void RunProcess(string command)
        {
            logger.WriteMessage(command);

            using (Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = string.Format("/c {0}", command),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                }
            })
            {
                process.Start();

                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    logger.WriteMessage(line);
                }
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;

            if (listener.IsListening)
            {
                listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                HttpListenerContext context = listener.EndGetContext(result);
                IWebServerRequest request = webServerRequestFactory.Create(context.Request);
                IWebServerResponse response = webServerResponseFactory.Create(context.Response);
                webSites.First(w => w.CanHandle(request)).Handle(request, response);
                logger.LogRequest(request, response);
                response.HttpListenerResponse.Close();
            }
        }

        public void Stop()
        {
            if (listener == null)
            {
                throw new InvalidOperationException("The web server has never been started");
            }

            if (!listener.IsListening)
            {
                throw new InvalidOperationException("The webs server has already been stopped");
            }

            listener.Stop();
            DeleteHttpNameSpaceReservations();
            logger.WriteMessage("Web server stopped");
        }

        public bool IsStarted
        {
            get
            {
                return (listener != null && listener.IsListening);
            }
        }
    }
}
