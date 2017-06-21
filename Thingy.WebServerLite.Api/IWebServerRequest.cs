using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public interface IWebServerRequest
    {
        /// <summary>
        /// The URL minus the scheme, host, port, (possibly) site name and query fields with
        /// system file separators
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// The name of the web site that is handling this request
        /// </summary>
        string HandlingWebSiteName { get; set; }

        /// <summary>
        /// The first element specified after the endpoint
        /// </summary>
        string WebSiteName { get; }
    }
}
