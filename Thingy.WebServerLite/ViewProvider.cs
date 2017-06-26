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
            string sectionDirectory = Path.Combine(viewsDirectory, request.ViewTemplateSection);
            string sectionMasterPage = Directory.GetFiles(sectionDirectory, "Master.*").FirstOrDefault();
            string viewTemplateFilter = string.Format("{0}.*", request.ViewTemplateName);
            string viewTemplatePage = Directory.GetFiles(sectionDirectory, viewTemplateFilter).FirstOrDefault();

            //TODO - Caching

            return viewFactory.Create(masterPage, sectionMasterPage, viewTemplatePage);
        }
    }
}
