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
        public string Index()
        {
            return "This is the return value from the DefaultController.Index action";
        }
    }
}
