using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class ViewReader : IDisposable
    {
        private StreamReader reader;
        private StringBuilder contentBuilder;

        private string buffer;

        public ViewReader(string path)
        {
            reader = new StreamReader(path);
            EndOfView = reader.EndOfStream;
            contentBuilder = new StringBuilder();
        }

        public bool EndOfView { get; private set; }

        public void Dispose()
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
        }

        internal string ReadContent()
        {
            contentBuilder.Clear();
            int delimiterPos = -1;
            Read();

            while (delimiterPos == -1 && !EndOfView)
            {
                delimiterPos = buffer.IndexOf('{');

                if (delimiterPos == -1)
                {
                    contentBuilder.AppendLine(buffer);
                    buffer = string.Empty;
                }
                else
                {
                    if (delimiterPos > 0)
                    {
                        contentBuilder.Append(buffer.Substring(0, delimiterPos));
                        buffer = buffer.Substring(delimiterPos);
                    }
                }

                Read();
            }

            return contentBuilder.ToString();
        }

        internal string ReadTemplate()
        {
            contentBuilder.Clear();
            int nestingLevel = 0;

            do
            {
                Read(); // !!! I don't think this is going to work if another template starts on the same line as the previous one ends !!!

                if (!string.IsNullOrEmpty(buffer))
                {
                    int openPos = buffer.IndexOf('{');

                    if (openPos != -1) // If there's any kind of open on the line then just write up to the open to the content, increase the nesting level and loop back
                                       // What about {#Template} {#Template}??
                    {
                        contentBuilder.Append(buffer.Substring(0, openPos + (nestingLevel > 0 ? 1 : 0)));
                        buffer = buffer.Substring(openPos + 1);
                        nestingLevel++;
                    }
                    else
                    {
                        int closePos = buffer.IndexOf('}');

                        if (closePos == -1) // There were no {} in the line at all. Write the whole buffer to content and loop back
                        {
                            contentBuilder.Append(buffer);
                            contentBuilder.Append(Environment.NewLine);
                            buffer = string.Empty;
                        }
                        else // Write up to the close } to the content and reduce the nesting level
                        {
                            contentBuilder.Append(buffer.Substring(0, closePos + (nestingLevel > 1 ? 1 : 0)));
                            buffer = buffer.Substring(closePos + 1);
                            nestingLevel--;
                        }
                    }
                }
            }
            while (nestingLevel != 0 && !EndOfView);

            if (nestingLevel != 0)
            {
                throw new ViewException("Error reading template. No matching closing }");
            }

            return contentBuilder.ToString();
        }

        private void Read()
        {
            if (string.IsNullOrEmpty(buffer))
            {
                buffer = reader.ReadLine();
                EndOfView = string.IsNullOrEmpty(buffer) && reader.EndOfStream;
            }
        }
    }
}
