using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class ViewResult : IViewResult
    {
        public string Content { get; set; }
        public string ContentType { get; set; }
    }
}
