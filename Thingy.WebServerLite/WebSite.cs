using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class WebSite : IWebSite
    {
        private readonly string name;
        private readonly IController[] controllers;
        private readonly string path;

        public WebSite(IController[] controllers, string name, int portNumber, string path)
        {
            this.name = name;
            this.PortNumber = portNumber;
            this.path = path;
            this.controllers = controllers.Where(c => c.GetType().Assembly.GetName().Name.EndsWith(name)).OrderBy(c => c.Priority).ToArray();
            IsDefault = false;
            Priority = Priorities.Normal;
        }

        public bool IsDefault { get; set; }

        public int PortNumber { get; private set; }

        public Priorities Priority { get; set; }

        public bool CanHandle(IWebServerRequest request)
        {
            return (IsDefault || name == request.WebSiteName);
        }

        public void Handle(IWebServerRequest request, IWebServerResponse response)
        {
            request.HandlingWebSiteName = name;

            IController controller = controllers.FirstOrDefault(c => c.CanHandle(request));

            if (controller!=null)
            {
                controller.Handle(request, response);
            }
            else
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
        }
    }
}
