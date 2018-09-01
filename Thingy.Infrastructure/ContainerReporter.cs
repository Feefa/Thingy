using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Castle.MicroKernel.Registration;

namespace Thingy.Infrastructure
{
    internal static class ContainerReporter
    {
        /// <summary>
        /// Internal list of types considered by the convention-based resolver
        /// </summary>
        private static IList<Type> typeList = new List<Type>();

        /// <summary>
        /// Writes a diagnostic dump of everything installed in the Castle Windsor container
        /// </summary>
        internal static void Dump()
        {
            if (!string.IsNullOrEmpty(InfrastructureConfiguration.DumpFilePath))
            {
                StreamWriter streamWriter = new StreamWriter(InfrastructureConfiguration.DumpFilePath);
                DumpAssignableHandlers(streamWriter);
                DumpTypes(streamWriter);

                streamWriter.Flush();
                streamWriter.Close();
            }

        }

        /// <summary>
        /// Dumps all the types considered by the convention-based installer
        /// </summary>
        /// <param name="streamWriter">A stream writer to write the dump to</param>
        private static void DumpTypes(StreamWriter streamWriter)
        {
            streamWriter.WriteLine("--------------------------------------------------------------------------------");
            streamWriter.WriteLine("Types");
            streamWriter.WriteLine("--------------------------------------------------------------------------------");
            foreach (Type t in typeList)
            {
                streamWriter.WriteLine("{1}.{0}, {2}", t.Name, t.Namespace, t.AssemblyQualifiedName);
            }
            streamWriter.WriteLine("--------------------------------------------------------------------------------");
            streamWriter.WriteLine("");
        }

        /// <summary>
        /// Dumps all the classes that have been detected as implementing services along with any associated parameters
        /// </summary>
        /// <param name="streamWriter">A stream writer to write the dump to</param>
        private static void DumpAssignableHandlers(StreamWriter streamWriter)
        {
            foreach (var handler in Bootstrapper.Container.Kernel.GetAssignableHandlers(typeof(object)))
            {
                streamWriter.WriteLine("--------------------------------------------------------------------------------");
                streamWriter.WriteLine("{0} (id=\"{1}\") implements the following services", handler.ComponentModel.Implementation, handler.ComponentModel.Implementation.Name);
                streamWriter.WriteLine("--------------------------------------------------------------------------------");
                foreach (var service in handler.ComponentModel.Services)
                {
                    streamWriter.WriteLine(service);
                }

                if (handler.ComponentModel.Parameters.Count > 0)
                {
                    streamWriter.WriteLine("--------------------------------------------------------------------------------");
                    streamWriter.WriteLine("Parameters");
                    streamWriter.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var p in handler.ComponentModel.Parameters)
                    {
                        streamWriter.WriteLine("- {0} = {1}", p.Name, p.Value);
                    }

                    streamWriter.WriteLine("--------------------------------------------------------------------------------");
                }

                streamWriter.WriteLine("");
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
            if (InfrastructureConfiguration.NamespacePrefixes.Count == 0 || InfrastructureConfiguration.NamespacePrefixes.Any(n => p.Namespace.StartsWith(n)))
            {
                typeList.Add(p);
            }

            return true;
        }
    }
}
