using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public interface IWebServer
    {
        bool IsStarted { get; }

        void Start();

        void Stop();
    }
}
