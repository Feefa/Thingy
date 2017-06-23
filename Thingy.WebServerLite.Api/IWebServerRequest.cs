using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        /// The web site that is handling this request
        /// </summary>
        IWebSite WebSite { get; set; }

        /// <summary>
        /// The first element specified after the endpoint
        /// </summary>
        string WebSiteName { get; }

        /// <summary>
        /// The second element specified after the endpoint
        /// </summary>                 
        string ControllerName { get; }

        /// <summary>
        /// The third element specified after the endpoint
        /// </summary>
        string ControllerMethodName { get; }

        /// <summary>
        /// The Http Method from the request
        /// </summary>
        string HttpMethod { get; }

        IUser User { get; }

        /// <summary>
        /// The values in the URL
        /// </summary>
        string[] UrlValues { get; }

        /// <summary>
        /// Represents a unified view of the query and/or content fields
        /// </summary>
        IDictionary<string, string> Fields { get; }
    }
}
