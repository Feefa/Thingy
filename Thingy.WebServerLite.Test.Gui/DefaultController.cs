using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite.Test.Gui
{
    public class DefaultController : ControllerBase
    {
        [HttpMethod("GET")]
        public string[] Index()
        {
            ////Request.ViewTemplateName = "Switch";

            return new string[] { "This is the return value from the DefaultController.Index action", "It now spans multiple lines", "Can we render all of them?"};
        }
    }
}
