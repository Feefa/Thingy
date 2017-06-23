using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HttpMethodAttribute : Attribute
    {
        public HttpMethodAttribute(string supports)
        {
            Supports = supports;
        }

        public string Supports { get; private set; }
    }
}
