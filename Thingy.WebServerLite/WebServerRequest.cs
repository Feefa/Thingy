using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    /// <summary>
    /// Implementation of IWebServerRequest that populates its properties by
    /// parsing the incoming HttpListenerRequest's url and body
    /// </summary>
    public class WebServerRequest : IWebServerRequest
    {
        private readonly char[] ampersand = new char[] { '&' };
        private readonly char[] equal = new char[] { '=' };
        private readonly char[] slash = new char[] { '/' };

        private string[] urlValues;
        private string viewTemplateName;
        private string viewTemplateSection;

        /// <summary>
        /// Create a new instance of the WebServerRequestClass
        /// </summary>
        /// <param name="userProvider">An instance of an IUserProvider implementation</param>
        /// <param name="request">The HttpListenerRequest to populate the properties from</param>
        public WebServerRequest(IUserProvider userProvider, HttpListenerRequest request)
        {
            this.HttpListenerRequest = request;
            ParseRequestUrl();
            ParseRequestBody();
            AuthenticateUser(userProvider);
        }

        /// <summary>
        /// Parse the HttpListenerRequest's body into the Fields dictionary
        /// </summary>
        private void ParseRequestBody()
        {
            if (HttpListenerRequest.HasEntityBody)
            {
                if (HttpListenerRequest.ContentType == "application/x-www-form-urlencoded")
                {
                    using (Stream stream = HttpListenerRequest.InputStream)
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string content = reader.ReadToEnd();

                        foreach (string[] nameValuePair in content.Split(ampersand).Select(n => n.Split(equal)))
                        {
                            Fields[nameValuePair[0]] = nameValuePair[1];
                        }

                        reader.Close();
                        stream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Extract the user information from the requset and user the IUserProvider implementation to authenticate it
        /// </summary>
        /// <param name="userProvider">The user provider</param>
        private void AuthenticateUser(IUserProvider userProvider)
        {
            string ipAddress = HttpListenerRequest.RemoteEndPoint.Address.ToString();
            string userId = FieldOrDefault("userid");
            string password = FieldOrDefault("password");
            User = userProvider.GetUser(ipAddress, userId, password);
        }

        /// <summary>
        /// Parse the HttpListenerRequest's Url into various properties
        /// </summary>
        private void ParseRequestUrl()
        {
            SetDefaultValues();

            if (HttpListenerRequest.Url.Segments.Length > 0)
            {
                IsFile = HttpListenerRequest.Url.Segments[HttpListenerRequest.Url.Segments.Length - 1].Contains(".");

                if (IsFile)
                {
                    SetValuesForFileUrl();
                }
                else
                {
                    SetValuesForControllerUrl();
                }
            }

            AddQueryFields();
        }

        /// <summary>
        /// Add query fields from the HttpListenerRequest's Url into the Fields dictionary
        /// </summary>
        private void AddQueryFields()
        {
            if (!string.IsNullOrEmpty(HttpListenerRequest.Url.Query))
            {
                foreach (string[] nameValuePair in HttpListenerRequest.Url.Query.Substring(1).Split(ampersand).Select(n => n.Split(equal)))
                {
                    Fields[nameValuePair[0]] = nameValuePair[1];
                }
            }
        }

        /// <summary>
        /// Set property values from a Url that appears to represent a Controller method call
        /// </summary>
        private void SetValuesForControllerUrl()
        {
            if (HttpListenerRequest.Url.Segments.Length > 1)
            {
                ControllerName = HttpListenerRequest.Url.Segments[1].TrimEnd(slash);

                if (HttpListenerRequest.Url.Segments.Length > 2)
                {
                    ControllerMethodName = HttpListenerRequest.Url.Segments[2].TrimEnd(slash);

                    if (HttpListenerRequest.Url.Segments.Length > 3)
                    {
                        GetUrlValues();
                    }
                }

                for (int i = 1; i < HttpListenerRequest.Url.Segments.Length; i++) // Fallback file path if the controller isn't found
                {
                    FilePath = Path.Combine(FilePath, HttpListenerRequest.Url.Segments[i].TrimEnd(slash));
                }
            }
        }

        /// <summary>
        /// Get UrlValues from a Url that appears to represent a Controller method call
        /// </summary>
        private void GetUrlValues()
        {
            Array.Resize(ref urlValues, HttpListenerRequest.Url.Segments.Length - 3);

            for (int i = 3; i < HttpListenerRequest.Url.Segments.Length; i++)
            {
                UrlValues[i - 3] = HttpListenerRequest.Url.Segments[i].TrimEnd(slash);
            }
        }

        /// <summary>
        /// Get property values from a Url that appears to represent a file name
        /// </summary>
        private void SetValuesForFileUrl(int startFrom = 1)
        {
            FilePath = string.Empty;

            if (HttpListenerRequest.Url.Segments.Length > startFrom)
            {
                for (int i = startFrom; i < HttpListenerRequest.Url.Segments.Length; i++)
                {
                    FilePath = Path.Combine(FilePath, HttpListenerRequest.Url.Segments[i].Trim(slash));
                }
            }
        }

        /// <summary>
        /// Set the default values for the properties that will pass through if nothing
        /// in the Url overrides them
        /// </summary>
        private void SetDefaultValues()
        {
            ControllerMethodName = string.Empty;
            ControllerName = string.Empty;
            FilePath = string.Empty;
            urlValues = new string[] { };
            Fields = new Dictionary<string, string>();
        }

        /// <summary>
        /// Changes a controller type request into a file request by adding a page name and setting IsFile
        /// </summary>
        /// <param name="fileName">The file name</param>
        public void SetFileName(string fileName)
        {
            if (!IsFile)
            {
                FilePath = Path.Combine(FilePath, fileName);
                IsFile = true;
            }
        }

        /// <summary>
        /// Gets the string from the Fields dictionary with the give name or an empty string if it does not exist
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <returns>The value of the field or an empty string if it does not exist</returns>
        public string FieldOrDefault(string name)
        {
            if (Fields.ContainsKey(name))
            {
                return Fields[name];
            }

            return string.Empty;
        }

        /// <summary>
        /// Remove the controller name from the file path if it is present
        /// </summary>
        public void AdjustFilePathForController()
        {
            if (HttpListenerRequest.Url.Segments[1].Trim(slash).Equals(ControllerName, StringComparison.OrdinalIgnoreCase))
            {
                SetValuesForFileUrl(2);
            }
        }

        /// <summary>
        /// The part of the URL that represents the controller method name
        /// </summary>
        public string ControllerMethodName { get; set; }

        /// <summary>
        /// The part of the URL that represents the controller name
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// The part of the URL that represents a file path
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// The HTTP Method
        /// </summary>
        public string HttpMethod
        {
            get
            {
                return HttpListenerRequest.HttpMethod;
            }
        }

        /// <summary>
        /// The User
        /// </summary>
        public IUser User { get; private set; }

        /// <summary>
        /// The web site that is handling the request
        /// </summary>
        public IWebSite WebSite { get; set; }

        /// <summary>
        /// Any values specified after the controller methods name in the Url
        /// </summary>
        public string[] UrlValues
        {
            get
            {
                return urlValues;
            }
        }

        /// <summary>
        /// Any values specified in the QueryFields or Body
        /// </summary>
        public IDictionary<string, string> Fields { get; private set; }

        /// <summary>
        /// True if the request parser determines this to be a file Url
        /// </summary>
        public bool IsFile { get; private set; }

        /// <summary>
        /// The ViewTemplateName
        /// Defaults to the ControllerMethodName if its backing field is empty
        /// </summary>
        public string ViewTemplateName
        {
            get
            {
                return string.IsNullOrEmpty(viewTemplateName) ? ControllerMethodName : viewTemplateName;
            }

            set
            {
                viewTemplateName = value;
            }
        }

        /// <summary>
        /// The ViewTemplateSection
        /// Defaults to the ControllerMethodName if its backing field is empty
        /// </summary>
        public string ViewTemplateSection
        {
            get
            {
                return string.IsNullOrEmpty(viewTemplateSection) ? ControllerName : viewTemplateSection;
            }

            set
            {
                viewTemplateSection = value;
            }
        }

        public HttpListenerRequest HttpListenerRequest { get; private set; }
    }
}
