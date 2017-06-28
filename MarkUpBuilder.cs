using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite
{
    public class MarkUpBuilder
    {
        private readonly StringBuilder stringBuilder;

        public MarkUpBuilder()
        {
            this.stringBuilder = new StringBuilder();
        }

        public MarkUpBuilder Append(string text)
        {
            stringBuilder.Append(text);

            return this;
        }

        public MarkUpBuilder AppendFormat(string formatString, params object[] values)
        {
            return Append(string.Format(formatString, values));
        }
    }
}
