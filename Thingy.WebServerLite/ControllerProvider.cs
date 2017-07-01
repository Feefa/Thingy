using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class ControllerProvider : IControllerProvider
    {
        private readonly IController[] controllers;

        public ControllerProvider(IController[] controllers)
        {
            this.controllers = controllers.OrderBy(c => c.Priority).ToArray();
        }

        public IController GetControllerForRequest(IWebServerRequest request)
        {
            return controllers.FirstOrDefault(c => c.CanHandle(request));
        }

        public IController GetNextControllerForRequest(IWebServerRequest request, Priorities prreviousPiority)
        {
            return controllers.FirstOrDefault(c => c.Priority > prreviousPiority && c.CanHandle(request));
        }
    }
}
