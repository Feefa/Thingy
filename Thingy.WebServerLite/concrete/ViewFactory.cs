using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class ViewFactory : IViewFactory
    {
        private readonly IViewLibrary[] commandLibraries;
        private readonly IMimeTypeProvider mimeTypeProvider;

        public ViewFactory(IViewLibrary[] commandLibraries, IMimeTypeProvider mimeTypeProvider)
        {
            this.commandLibraries = commandLibraries;
            this.mimeTypeProvider = mimeTypeProvider;
        }

        public IView Create(string masterPage, string controllerMasterPage, string controllerMethodPage)
        {
            return new View(commandLibraries, mimeTypeProvider, masterPage, controllerMasterPage, controllerMethodPage);
        }
    }
}
