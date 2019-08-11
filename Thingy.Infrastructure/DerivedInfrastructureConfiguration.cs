using System;
using System.Collections.Generic;
using System.IO;

namespace Thingy.Infrastructure
{
    /// <summary>
    /// DevivedInfrastructureConfiguration
    /// 
    /// Properties and methods by which classes in this assembly access the configurable properties defined by InfrastructureConfiguration
    /// 
    /// See comments in InfrastructureConfiguration.cs for information on how this library can be configured.
    /// </summary>
    internal static class DerivedInfrastructureConfiguration
    {
        /// <summary>
        /// Gets the main Assembly Directory. This is used for several purposes.
        /// o   The location where we search for assemblies containing classes eligible for convention-based registration.
        /// o   The location where we expect to find the /config directory containing XML configuration.
        /// o   The location where we will write a log file containing diagnostic information.
        /// 
        /// AssemblyDirectory will be the value of AppDomain.Current.BaseDirectory unless that directory contains a directory
        /// named 'bin', in which case Assembly will be the path to that \bin directory.
        /// </summary>
        internal static string AssemblyDirectory
        {
            get
            {
                string pathToBin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

                return Directory.Exists(pathToBin) ? pathToBin : AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// AssemblyFilterMask - Only assemblies matching this filter will be checked for Castle Windsor installers
        /// </summary>
        internal static string AssemblyFilterMask
        {
            get
            {
                return InfrastructureConfiguration.InstallerAssemblyFilterMask;
            }
        }

        /// <summary>
        /// Namespace prefix for assemblies to check for by convention registration
        /// </summary>
        internal static List<string> NamespacePrefixes
        {
            get
            {
                return InfrastructureConfiguration.ConventionBasedInstallerNamespacePrefixes;
            }
        }

        /// <summary>
        /// Path to write the castle windsor configuration dump
        /// If the configured value contains :\ then it will be interpreted as a full file name and returned as is,
        /// otherwise it will be appended to the assembly directory
        /// </summary>
        internal static string DumpFilePath
        {
            get
            {                
                return InfrastructureConfiguration.CastleWindsorDumpFileName.Contains(":\\") ?
                    InfrastructureConfiguration.CastleWindsorDumpFileName :                    
                    Path.Combine(AssemblyDirectory, InfrastructureConfiguration.CastleWindsorDumpFileName);
            }
        }

        /// <summary>
        /// Gets the path to the XML Config File directory
        /// If the configured value contains :\ then it will be interpreted as a full file name and returned as is,
        /// otherwise it will be appended to the assembly directory
        /// </summary>
        internal static string XmlConfigDirectory
        {
            get
            {
                return InfrastructureConfiguration.CastleWindsorConfigurationFolderName.Contains(":\\") ?
                    InfrastructureConfiguration.CastleWindsorConfigurationFolderName :
                    Path.Combine(AssemblyDirectory, InfrastructureConfiguration.CastleWindsorConfigurationFolderName);
            }
        }

        /// <summary>
        /// Gets the pattern used to identify XML config files
        /// </summary>
        internal static string XmlConfigPattern
        {
            get
            {
                return InfrastructureConfiguration.CastleWindsorConfigurationFilePattern;
            }
        }

        /// <summary>
        /// Writes the values of all the derived settings for inclusion in the log
        /// </summary>
        internal static void DumpConfiguration()
        {
            ContainerReporter.AddDiagnosticMessage("Derived Configuration Properties");
            ContainerReporter.AddDiagnosticMessage(string.Format("XmlConfigDirectory={0}", XmlConfigDirectory));
            ContainerReporter.AddDiagnosticMessage(string.Format("XmlConfigPattern={0}", XmlConfigPattern));
            ContainerReporter.AddDiagnosticMessage(string.Format("AssemblyDirectory={0}", AssemblyDirectory));
            ContainerReporter.AddDiagnosticMessage(string.Format("AssemblyFilterMask={0}", AssemblyFilterMask));
            ContainerReporter.AddDiagnosticMessage(string.Format("DumpFilePath={0}", DumpFilePath));

            foreach (string n in NamespacePrefixes)
            {
                ContainerReporter.AddDiagnosticMessage(string.Format("NamespacePrefixes[]={0}", n));
            }
        }

    }
}
