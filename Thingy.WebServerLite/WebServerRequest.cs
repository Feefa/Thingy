using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class WebServerRequest : IWebServerRequest
    {
        private readonly HttpListenerRequest request;

        public WebServerRequest(IUserProvider userProvider, HttpListenerRequest request)
        {
            this.request = request;
            ParseRequest();
            AuthenticateUser(userProvider);
        }

        private void AuthenticateUser(IUserProvider userProvider)
        {
            string ipAddress = request.RemoteEndPoint.Address.ToString();
            string userId = Fields["userid"];
            string password = Fields["password"];
            User = userProvider.GetUser(ipAddress, userId, password);
        }

        private void ParseRequest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The part of the URL that represents the controller method name
        /// </summary>
        public string ControllerMethodName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The part of the URL that represents the controller name
        /// </summary>
        public string ControllerName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The part of the URL that represents a file path
        /// </summary>
        public string FilePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The HTTP Method
        /// </summary>
        public string HttpMethod
        {
            get
            {
                return request.HttpMethod;
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
        /// The part of the URL that represents the web site name
        /// </summary>
        public string WebSiteName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string[] UrlValues
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDictionary<string, string> Fields
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
