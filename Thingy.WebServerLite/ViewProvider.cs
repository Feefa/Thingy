using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class ViewProvider : IViewProvider
    {
        private readonly IViewFactory viewFactory;

        public ViewProvider(IViewFactory viewFactory)
        {
            this.viewFactory = viewFactory;
        }

        public IView GetViewForRequest(IWebServerRequest request)
        {
            string viewsDirectory = request.WebSite.GetOsFilePath("Views");
            string masterPage = Directory.GetFiles(viewsDirectory, "Master.*").FirstOrDefault();
            string controllerDirectory = Path.Combine(viewsDirectory, request.ControllerName);
            string controllerMasterPage = Directory.GetFiles(controllerDirectory, "Master.*").FirstOrDefault();
            string methodPageFilter = string.Format("{0}.*", request.ControllerMethodName);
            string controllerMethodPage = Directory.GetFiles(controllerDirectory, methodPageFilter).FirstOrDefault();

            //TODO - Caching

            return viewFactory.Create(masterPage, controllerMasterPage, controllerMethodPage);
        }
    }
}
