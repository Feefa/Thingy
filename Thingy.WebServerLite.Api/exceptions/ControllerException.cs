using System;

namespace Thingy.WebServerLite.Api
{
    public class ControllerException : Exception
    {
        public ControllerException(string message) : base(message) { }
    }
}
