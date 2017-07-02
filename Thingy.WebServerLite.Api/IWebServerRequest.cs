using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public interface IWebServerRequest
    {
        HttpListenerRequest HttpListenerRequest { get; }

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
        /// The second element specified after the endpoint
        /// </summary>                 
        string ControllerName { get; set; }

        /// <summary>
        /// The third element specified after the endpoint
        /// </summary>
        string ControllerMethodName { get; set; }

        /// <summary>
        /// The name of the view template. Default to ControllerMethodName if not specified
        /// </summary>
        string ViewTemplateName { get; set; }

        /// <summary>
        /// Adjust the file path according to remove the controller name if it is present
        /// </summary>
        void AdjustFilePathForController();

        /// <summary>
        /// The name of the view template section. Default to ControllerName if not specified
        /// </summary>
        string ViewTemplateSection { get; set; }

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

        bool IsFile { get; }

        /// <summary>
        /// Set the file name in the FilePath and change the request
        /// into a file request
        /// </summary>
        /// <param name="fileName">The file name</param>
        void SetFileName(string fileName);

        /// <summary>
        /// Gets the string from the Fields dictionary with the give name or an empty string if it does not exist
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <returns>The value of the field or an empty string if it does not exist</returns>
        string FieldOrDefault(string name);
    }
}
