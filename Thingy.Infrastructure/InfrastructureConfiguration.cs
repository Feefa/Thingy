using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Thingy.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static InfrastructureConfiguration()
        {
            CastleWindsorConfigurationFolderName = "config";
            CastleWindsorConfigurationFilePattern = "*.xml";
            CastleWindsorDumpFileName = "CastleWindsor.dump.log";
            // When this is moved to a library is needs to initialise this collection to contain its namespace
            // Or perhaps a higher-level library namespace so it automatically picks up installers in library code
            ConventionBasedInstallerNamespacePrefixes = new List<string>();
            InstallerAssemblyFilterMask = "*";
        }

        /// <summary>
        /// Name of the folder within the bin directory where the Castle Windsor config is stored
        /// </summary>
        public static string CastleWindsorConfigurationFolderName { get; set; }

        /// <summary>
        /// Pattern to match to select Castle Windsor configuration files
        /// </summary>
        public static string CastleWindsorConfigurationFilePattern { get; set; }

        /// <summary>
        /// Name of the Castle Windsor dump log file - if this is an empty string then the log will not be written
        /// </summary>
        public static string CastleWindsorDumpFileName { get; set; }

        /// <summary>
        /// Assembly filter mask - Only assemblies with names matching this filter will be checked for installers and used by the convention-based installer
        /// </summary>
        public static string InstallerAssemblyFilterMask { get; set; }

        /// <summary>
        /// Convention-based installer namespace prefix - Only classes in namespaces with this prefix will be wired by convention
        /// </summary>
        public static List<string> ConventionBasedInstallerNamespacePrefixes { get; set; }

        /// <summary>
        /// Gets the path to the XML Config File
        /// </summary>
        internal static string XmlConfigDirectory
        {
            get
            {
                return Path.Combine(AssemblyDirectory, CastleWindsorConfigurationFolderName);
            }
        }

        internal static string XmlConfigPattern 
        {
            get
            {
                return CastleWindsorConfigurationFilePattern;
            }
        }

        /// <summary>
        /// Gets the path to the assemblies that we will search for installers
        /// </summary>
        internal static string AssemblyDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        /// <summary>
        /// AssemblyFilterMask - Only assemblies matching this filter will be checked for Castle Windsor installers
        /// </summary>
        internal static string AssemblyFilterMask
        {
            get
            {
                return InstallerAssemblyFilterMask;
            }
        }

        /// <summary>
        /// Path to write the castle windsor configuration dump
        /// </summary>
        internal static string DumpFilePath 
        {
            get
            {
                return Path.Combine(AssemblyDirectory, CastleWindsorDumpFileName);
            }
        }

        /// <summary>
        /// Namespace prefix for assemblies to check for by convention registration
        /// </summary>
        internal static List<string> NamespacePrefixes 
        {
            get
            {
                return ConventionBasedInstallerNamespacePrefixes;
            }
        }
    }
}
