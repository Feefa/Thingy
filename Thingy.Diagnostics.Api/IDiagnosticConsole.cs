using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.Diagnostics.Api
{
    public interface IDiagnosticConsole
    {
        /// <summary>
        /// Write a diagnostic message
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="method">The name of the method</param>
        /// <param name="level">The diagnostic level</param>
        /// <param name="message">The message</param>
        void WriteMessage(object source, string method, DiagnosticLevels level, string message);
    }
}
