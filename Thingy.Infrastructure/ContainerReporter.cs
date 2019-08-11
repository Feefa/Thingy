using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Thingy.Infrastructure
{
    /// <summary>
    /// Methods used internally in this library for logging and diagnostics
    /// </summary>
    internal static class ContainerReporter
    {
        /// <summary>
        /// Internal list of types considered by the convention-based resolver
        /// </summary>
        private static IList<Type> typeList = new List<Type>();

        private const string horizontalLine = "----------------------------------------------------------------------------------------------------------------------------------------------------------------";

        /// <summary>
        /// Internal list of diagnostic messages written during the session
        /// </summary>
        private static IList<string> diagnosticMesages = new List<string>();

        /// <summary>
        /// Writes a diagnostic dump of everything installed in the Castle Windsor container
        /// </summary>
        internal static void Dump()
        {
            if (!string.IsNullOrEmpty(DerivedInfrastructureConfiguration.DumpFilePath))
            {
                using (StreamWriter streamWriter = new StreamWriter(DerivedInfrastructureConfiguration.DumpFilePath, !InfrastructureConfiguration.ClearLogFile))
                {
                    streamWriter.WriteLine(horizontalLine);
                    streamWriter.WriteLine("-- Diagnostic Messages");
                    streamWriter.WriteLine(horizontalLine);
                    DumpDiagnosticMessages(streamWriter);
                    streamWriter.WriteLine(horizontalLine);
                    streamWriter.WriteLine("-- Registered Classes with services and parameters");
                    streamWriter.WriteLine(horizontalLine);
                    DumpAssignableHandlers(streamWriter);

                    if (InfrastructureConfiguration.DumpAllTypes)
                    {
                        DumpTypes(streamWriter);
                    }

                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

        }

        /// <summary>
        /// Dump the contents of the diagnostic messages list
        /// </summary>
        /// <param name="writer">A stream writer to write the messages to</param>
        private static void DumpDiagnosticMessages(StreamWriter writer)
        {
            foreach (string line in diagnosticMesages)
            {
                writer.WriteLine(line);
            }
        }

        /// <summary>
        /// Dumps all the types considered by the convention-based installer
        /// </summary>
        /// <param name="streamWriter">A stream writer to write the dump to</param>
        private static void DumpTypes(StreamWriter streamWriter)
        {
            streamWriter.WriteLine(horizontalLine);
            streamWriter.WriteLine("-- Types");
            streamWriter.WriteLine(horizontalLine);
            foreach (Type t in typeList)
            {
                streamWriter.WriteLine("{1}.{0}, {2}", t.Name, t.Namespace, t.AssemblyQualifiedName);
            }
            streamWriter.WriteLine(horizontalLine);
            streamWriter.WriteLine("");
        }

        /// <summary>
        /// Dumps all the classes that have been detected as implementing services along with any associated parameters
        /// </summary>
        /// <param name="streamWriter">A stream writer to write the dump to</param>
        private static void DumpAssignableHandlers(StreamWriter streamWriter)
        {
            foreach (IHandler handler in Bootstrapper.Container.Kernel.GetAssignableHandlers(typeof(object)))
            {
                streamWriter.WriteLine("{0} (id=\"{1}\") implements the following services", handler.ComponentModel.Implementation, handler.ComponentModel.Implementation.Name);

                foreach (var service in handler.ComponentModel.Services)
                {
                    streamWriter.WriteLine("  *  {0}",service);
                }

                if (handler.ComponentModel.Parameters.Count > 0)
                {
                    streamWriter.WriteLine("Parameters");
                    foreach (var p in handler.ComponentModel.Parameters)
                    {
                        streamWriter.WriteLine("  *   {0} = {1}", p.Name, p.Value);
                    }
                }

                streamWriter.WriteLine(horizontalLine);
            }
        }

        /// <summary>
        /// Slightly hacky way of getting at the types considered by the convention-based installer as they are enumerated.
        /// They are filtered by this method (always returns true) and added to the list as a side-effect
        /// </summary>
        /// <param name="p">The type</param>
        /// <returns>true - always</returns>
        internal static bool AddType(Type p)
        {
            if (DerivedInfrastructureConfiguration.NamespacePrefixes.Count == 0 || 
                DerivedInfrastructureConfiguration.NamespacePrefixes.Any(n => p.Namespace.StartsWith(n)))
            {
                typeList.Add(p);

                //// Also add the interfaces that each type implements - useful diagnostic but makes the log hard to understand.
                ////foreach(Type type in p.GetInterfaces())
                ////{
                ////    typeList.Add(type);
                ////}
            }

            return true;
        }

        /// <summary>
        /// Add diagnostic messages to a string list for inclusion in the final dump log
        /// </summary>
        /// <param name="text">The message</param>
        internal static void AddDiagnosticMessage(string text)
        {
            diagnosticMesages.Add(string.Format("{0} : {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"), text));
        }
    }
}
