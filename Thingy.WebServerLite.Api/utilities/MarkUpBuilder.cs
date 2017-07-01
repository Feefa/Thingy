using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public class MarkUpBuilder
    {
        private readonly StringBuilder stringBuilder;

        public MarkUpBuilder()
        {
            this.stringBuilder = new StringBuilder();
        }

        public MarkUpBuilder Append(params object[] values)
        {
            foreach (object value in values)
            {
                stringBuilder.Append(value.ToString());
            }

            return this;
        }

        public MarkUpBuilder AppendFormat(string formatString, params object[] values)
        {
            return Append(string.Format(formatString, values));
        }

        public MarkUpBuilder AppendConditional(bool append, params object[] values)
        {
            return append ? Append(values) : this;
        }
        public MarkUpBuilder AppendFormatConditional(bool append, string formatString, params object[] values)
        {
            return AppendConditional(append, string.Format(formatString, values));
        }

        public MarkUpBuilder AppendAttribute(string name, string value)
        {
            return Append(" ").Append(name).Append("=\"").Append(value).Append("\"");
        }

        public MarkUpBuilder AppendAttributeIfPopulated(string name, string value)
        {
            return string.IsNullOrEmpty(value) ? this : AppendAttribute(name, value);
        }

        ////public MarkUpBuilder If(bool condition)
        ////{
        ////    ifStack.Push(condition);

        ////    return this;
        ////}

        ////public MarkUpBuilder Else()
        ////{
        ////    ifStack.Push(!ifStack.Pop());

        ////    return this;
        ////}

        ////public MarkUpBuilder EndIf()
        ////{
        ////    ifStack.Pop();
        ////    return this;
        ////}

        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }
}
