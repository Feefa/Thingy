using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                listener.Prefixes.Add(string.Format("http://*:{0}/", webSite.PortNumber));
            }

            listener.Start();
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
            logger.WriteMessage("Web server started");
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
