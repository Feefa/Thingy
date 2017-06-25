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

        HttpListener listener = null;

        public WebServer(
            IWebSite[] webSites,
            IWebServerRequestFactory webServerRequestFactory,
            IWebServerResponseFactory webServerResponseFactory)
        {
            this.webSites = webSites.OrderBy(w => w.Priority).ToArray();
            this.webServerRequestFactory = webServerRequestFactory;
            this.webServerResponseFactory = webServerResponseFactory;
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
