using System;

namespace Thingy.WebServerLite.Api
{
    public class ViewException : Exception
    {
        public ViewException(string message) : base(message)
        {
        }

        public ViewException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
