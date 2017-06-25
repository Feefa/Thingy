using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    /// <summary>
    /// POC Library
    /// </summary>
    public class HtmlViewLibrary : IViewLibrary
    {
        /// <summary>
        /// POC Function
        /// </summary>
        /// <param name="name"></param>
        /// <param name="caption"></param>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public string Button(string name, string caption, string type = "button", string attributes = "")
        {
            return string.Format("<button name=\"{0}\" type=\"{2}\"{3}{4}>{1}</button>", name, caption, type, string.IsNullOrEmpty(attributes) ? string.Empty : " ", attributes);
        }
    }
}
