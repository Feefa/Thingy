﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class WebServerRequest : IWebServerRequest
    {
        private readonly HttpListenerRequest request;

        public WebServerRequest(HttpListenerRequest request)
        {
            this.request = request;
        }
    }
}
