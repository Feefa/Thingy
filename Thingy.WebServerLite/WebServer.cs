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
        private readonly bool isAdmin;

        HttpListener listener = null;

        public WebServer(
            IWebSite[] webSites,
            IWebServerRequestFactory webServerRequestFactory,
            IWebServerResponseFactory webServerResponseFactory,
            IWebServerLoggingProvider logger)
        {
            this.webSites = webSites.OrderByDescending(w => w.Priority).ToArray();
            this.webServerRequestFactory = webServerRequestFactory;
            this.webServerResponseFactory = webServerResponseFactory;
            this.logger = logger;
            this.isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

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

                if (isAdmin)
                {
                    AddHttpNameSpaceReservation(prefix);
                    AddFirewallRule(webSite.Name, webSite.PortNumber);
                }
                else
                {
                    logger.WriteMessage("You are not running in administrator mode. To get the web server to work you may need to enter the following commands in a command window with elevated privileges.");
                    logger.WriteMessage(GetAddNameSpaceReservationCommand(prefix));
                    logger.WriteMessage(GetAddFirewallRuleInCommand(webSite.Name, webSite.PortNumber));
                    logger.WriteMessage(GetAddFirewallRuleOutCommand(webSite.Name, webSite.PortNumber));
                    logger.WriteMessage("To remove these setttings:");
                    logger.WriteMessage(GetDeleteNameSpaceReservationCommand(prefix));
                    logger.WriteMessage(GetRemoveFirewallRuleInCommand(webSite.Name, webSite.PortNumber));
                    logger.WriteMessage(GetRemoveFirewallRuleOutCommand(webSite.Name, webSite.PortNumber));
                }
            }

            listener.Start();
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
            logger.WriteMessage("Web server started");
        }

        private void DeleteHttpNameSpaceReservations()
        {
            if (isAdmin && webSites != null)
            {
                foreach (IWebSite webSite in webSites)
                {
                    string prefix = string.Format("http://*:{0}/", webSite.PortNumber);
                    DeleteHttpNameSpaceReservation(prefix);
                    RemoveFireWallRule(webSite.Name, webSite.PortNumber);
                }
            }
        }

        private void AddHttpNameSpaceReservation(string prefix)
        {
            RunProcess(GetAddNameSpaceReservationCommand(prefix));
        }

        private static string GetAddNameSpaceReservationCommand(string prefix)
        {
            return string.Format("netsh http add urlacl url={0} user={1}", prefix, WindowsIdentity.GetCurrent().Name);
        }

        private void DeleteHttpNameSpaceReservation(string prefix)
        {
            RunProcess(GetDeleteNameSpaceReservationCommand(prefix));
        }

        private static string GetDeleteNameSpaceReservationCommand(string prefix)
        {
            return string.Format("netsh http delete urlacl url={0}", prefix);
        }

        private void AddFirewallRule(string name, int portNumber)
        {
            RunProcess(GetAddFirewallRuleInCommand(name, portNumber));
            RunProcess(GetAddFirewallRuleOutCommand(name, portNumber));
        }

        private static string GetAddFirewallRuleOutCommand(string name, int portNumber)
        {
            return string.Format("netsh advfirewall firewall add rule name=\"Game Server rule : {0}, open port {1}\" dir=out action=allow protocol=TCP localport={1}", name, portNumber);
        }

        private static string GetAddFirewallRuleInCommand(string name, int portNumber)
        {
            return string.Format("netsh advfirewall firewall add rule name=\"Game Server rule : {0}, open port {1}\" dir=in action=allow protocol=TCP localport={1}", name, portNumber);
        }

        private void RemoveFireWallRule(string name, int portNumber)
        {
            RunProcess(GetRemoveFirewallRuleInCommand(name, portNumber));
            RunProcess(GetRemoveFirewallRuleOutCommand(name, portNumber));
        }

        private static string GetRemoveFirewallRuleOutCommand(string name, int portNumber)
        {
            return string.Format("netsh advfirewall firewall delete rule name=\"Game Server rule : {0}, open port {1}\" dir=out protocol=TCP localport={1}", name, portNumber);
        }

        private static string GetRemoveFirewallRuleInCommand(string name, int portNumber)
        {
            return string.Format("netsh advfirewall firewall delete rule name=\"Game Server rule : {0}, open port {1}\" dir=in protocol=TCP localport={1}", name, portNumber);
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
