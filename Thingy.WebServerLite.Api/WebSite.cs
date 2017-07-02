using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public class WebSite : IWebSite
    {
        private readonly IControllerProvider controllerProvider;
        private readonly string path;

        private bool isDefault;

        public WebSite(IViewProvider viewProvider, IControllerProviderFactory controllerProviderFactory, IController[] controllers, string name, int portNumber, string path)
        {
            this.Name = name;
            this.PortNumber = portNumber;
            this.path = path;
            this.controllerProvider = controllerProviderFactory.Create(controllers.Where(c => c.GetType().Namespace.EndsWith(name)).ToArray());
            this.ViewProvider = viewProvider;
            IsDefault = false;
            Priority = Priorities.Normal;
            DefaultWebPage = "index.html";
        }

        public bool IsDefault
        {
            get
            {
                return isDefault;
            }

            set
            {
                isDefault = value;

                if (isDefault && Priority > Priorities.Low)
                {
                    Priority = Priorities.Low;
                }
            }
        }

        public int PortNumber { get; private set; }

        public Priorities Priority { get; set; }

        string DefaultWebPage { get; set; }        

        public IViewProvider ViewProvider { get; private set; }

        public string Name { get; private set; }

        public virtual bool CanHandle(IWebServerRequest request)
        {
            return (IsDefault || PortNumber == request.HttpListenerRequest.LocalEndPoint.Port);
        }

        public virtual void Handle(IWebServerRequest request, IWebServerResponse response)
        {
            request.WebSite = this;
            HandleControllerRequest(request, response);

            if (request.IsFile)
            {
                HandleFileRequest(request, response);
            }
        }

        protected void HandleFileRequest(IWebServerRequest request, IWebServerResponse response)
        {
            string filePath = Path.Combine(path, request.FilePath);

            if (File.Exists(filePath))
            {
                response.FromFile(filePath);
            }
            else
            {
                response.NotFound(request);
            }
        }

        protected void HandleControllerRequest(IWebServerRequest request, IWebServerResponse response)
        {
            IController controller = controllerProvider.GetControllerForRequest(request);

            if (controller == null || !controller.Handle(request, response))
            {
                request.SetFileName(DefaultWebPage);
            }
        }

        public string GetOsFilePath(string fileName)
        {
            return Path.Combine(path, fileName);
        }
    }
}
