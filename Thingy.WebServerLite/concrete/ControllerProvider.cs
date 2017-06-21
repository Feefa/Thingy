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
            this.controllers = controllers;
        }

        public IController GetControllerForRequest(IWebServerRequest request)
        {
            return controllers.FirstOrDefault(c => c.CanHandle(request));
        }
    }
}
