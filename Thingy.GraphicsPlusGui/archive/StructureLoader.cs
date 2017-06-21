using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.GraphicsPlusGui
{
    public class StructureLoader : IStructureLoader
    {
        private readonly string[] commentDelimiters = new string[] { "//" };
        private readonly char[] floatTrimDot = new char[] { '.' };
        private readonly char[] floatTrimZero = new char[] { '0' };
        private readonly char[] floatTrimF = new char[] { 'f' };

        public string Load(string fileName)
        {
            StringBuilder compiledContent = new StringBuilder();
            IList<string> minifiedNamesList = new List<string>();
            CompileFromFile(fileName, minifiedNamesList, compiledContent);

            return compiledContent.ToString();
        }

        private void CompileFromFile(string fileName, IList<string> minifiedNamesList, StringBuilder compiledContent, string namePrefix = null)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line = ReadDataLine(reader);

                while (!string.IsNullOrEmpty(line) || !reader.EndOfStream)
                {
                    if (line.Length > 0)
                    {
                        switch (line[0])
                        {
                            case 'J': CompileJoint(reader, line, minifiedNamesList, compiledContent, namePrefix); break;
                            case 'E': CompileElement(reader, line, minifiedNamesList, compiledContent, namePrefix); break;
                            case 'D': CompileDynamicJoint(reader, line, minifiedNamesList, compiledContent, namePrefix); break;
                            case 'I': IncludeJoint(reader, line, minifiedNamesList, compiledContent, namePrefix); break;
                            default: throw new StructureLoaderSyntaxErrorException("A non blank line must start with the characters '/', 'J', 'E', 'D' or 'I'.");
                        }
                    }

                    line = ReadDataLine(reader);
                }
            }
        }

        private string ReadDataLine(StreamReader reader)
        {
            string line = reader.ReadLine();

            while (line.StartsWith(commentDelimiters[0]))
            {
                line = reader.ReadLine();
            }

            return string.IsNullOrEmpty(line) ? line : line.Split(commentDelimiters, StringSplitOptions.None)[0].TrimEnd();
        }

        private void CompileJoint(StreamReader reader, string line, IList<string> minifiedNamesList, StringBuilder compiledContent, string namePrefix)
        {
            string[] parts = line.Split(' ');
            string name;

            if (parts.Length < 2)
            {
                name = namePrefix;
            }
            else
            {
                name = CombineNamePrefixAndName(namePrefix, parts[1]);
            }

            compiledContent.Append('J');

            if (!string.IsNullOrEmpty(name))
            {
                compiledContent.Append(' ');
                compiledContent.Append(GetMinifiedName(minifiedNamesList, name));
            }

            compiledContent.Append(";");
            CommonCompileJoint(reader, compiledContent);
        }

        private void CompileDynamicJoint(StreamReader reader, string line, IList<string> minifiedNamesList, StringBuilder compiledContent, string namePrefix)
        {
            string[] parts = line.Split(' ');
            string name = CombineNamePrefixAndName(namePrefix, parts[1]);
            compiledContent.Append(string.Format("D {0} {1};", GetMinifiedName(minifiedNamesList, name), parts[2]));
            CommonCompileJoint(reader, compiledContent);
        }

        private void CommonCompileJoint(StreamReader reader, StringBuilder compiledContent)
        {
            CompilePointsList(ReadDataLine(reader), compiledContent);
            CompileValuesList(ReadDataLine(reader), compiledContent);
            CompileValuesList(ReadDataLine(reader), compiledContent);
        }

        private void CompileElement(StreamReader reader, string line, IList<string> minifiedNamesList, StringBuilder compiledContent, string namePrefix)
        {
            string[] parts = line.Split(' ');
            string name = CombineNamePrefixAndName(namePrefix, parts[1]);
            compiledContent.Append(string.Format("E {0};", GetMinifiedName(minifiedNamesList, name)));
            compiledContent.Append(string.Format("{0};", ReadDataLine(reader)));
            CompilePointsList(ReadDataLine(reader), compiledContent);
            compiledContent.Append(string.Format("{0};", ReadDataLine(reader)));
            CompileValuesList(ReadDataLine(reader), compiledContent);
        }

        private void IncludeJoint(StreamReader reader, string line, IList<string> minifiedNamesList, StringBuilder compiledContent, string namePrefix)
        {
            string[] parts = line.Split(' ');
            string newNamePrefix = CombineNamePrefixAndName(namePrefix, parts[1]);
            CompileFromFile(parts[2], minifiedNamesList, compiledContent, newNamePrefix);
        }

        private string GetMinifiedName(IList<string> minifiedNamesList, string name)
        {
            string[] parts = name.Split('.');
            bool first = true;
            StringBuilder nameBuilder = new StringBuilder();

            foreach (string part in parts)
            {
                if (!minifiedNamesList.Contains(part))
                {
                    minifiedNamesList.Add(part);
                }

                int minifiedNameIndex = minifiedNamesList.IndexOf(part);

                if (first)
                {
                    first = false;
                }
                else
                {
                    nameBuilder.Append('.');
                }

                if (minifiedNameIndex > 25)
                {
                    nameBuilder.Append((char)(minifiedNameIndex / 26 + 97));
                }

                nameBuilder.Append((char)(minifiedNameIndex % 26 + 97));
            }

            return nameBuilder.ToString();
        }

        private string MinifiedFloatValue(string value)
        {
            string minifiedValue = value.ToLower().TrimEnd(floatTrimF).TrimEnd(floatTrimZero).TrimEnd(floatTrimDot);

            return string.IsNullOrEmpty(minifiedValue) ? "0" : minifiedValue;
        }

        private void CompilePointsList(string line, StringBuilder compiledContent)
        {
            string[] points = line.Split(' ');
            bool first = true;

            foreach (string point in points)
            {
                string[] values = point.Split(',');

                if (first)
                {
                    first = false;
                }
                else
                {
                    compiledContent.Append(' ');
                }

                compiledContent.Append(MinifiedFloatValue(values[0]));
                compiledContent.Append(',');
                compiledContent.Append(MinifiedFloatValue(values[1]));
            }

            compiledContent.Append(';');
        }

        private void CompileValuesList(string line, StringBuilder compiledContent)
        {
            string[] values = line.Split(' ');
            bool first = true;

            foreach (string value in values)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    compiledContent.Append(' ');
                }

                compiledContent.Append(MinifiedFloatValue(value));
            }

            compiledContent.Append(';');
        }

        private static string CombineNamePrefixAndName(string namePrefix, string name)
        {
            return string.IsNullOrEmpty(namePrefix) ? name : string.Format("{0}.{1}", namePrefix, name);
        }
    }
}
