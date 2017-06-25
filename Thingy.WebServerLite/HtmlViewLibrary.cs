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
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        string Button(string name, string value, string type = "button", string attributes = "")
        {
            return string.Format("<button name=\"{0}\" value=\"{1}\" type=\"{2}\"{3}{4} />", name, value, type, string.IsNullOrEmpty(attributes) ? string.Empty : " ", attributes);
        }
    }
}
