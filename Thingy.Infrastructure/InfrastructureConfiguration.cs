using System.Collections.Generic;

namespace Thingy.Infrastructure
{
    /// <summary>
    /// ----------------------------------------------------------------------------------------------------
    /// Configuration
    /// 
    /// This library has been designed in such a way that it can simply be referenced by the main project
    /// and it will perform convention-based registration with little or no further action required.
    /// 
    /// By default it will consider all classes defined in assemblies in the same folder as the main 
    /// assembly for convention-based registration so long as they are NOT defined in namespaces whose names
    /// begin 'Castle' or 'System'. 
    /// 
    /// A class will be registered as implementation of an interface if,
    /// o   It actually implements that interface
    /// o   Its name contains the name of the interface (without the I- prefix)
    /// ----------------------------------------------------------------------------------------------------
    /// CastleWindsorConfigurationFolderName
    /// 
    /// The name of a subfolder in the same folder as the main assembly where this library will look for XML
    /// configuration files.
    /// Update : This setting will now be interpreted as an absolute path if it contains the character 
    ///          sequence ":\"
    /// 
    /// Default value = 'config'
    /// ----------------------------------------------------------------------------------------------------
    /// CastleWindsorConfigurationFilePattern
    /// 
    /// The pattern that is applied to files in the configuration folder above to identify those that should
    /// be included.
    /// 
    /// Default value = '*.xml'
    /// ----------------------------------------------------------------------------------------------------
    /// CastleWindsorDumpFileName
    /// 
    /// The name of a file in the same folder as the main assembly where diagnostic information about the
    /// dependency resolution process will be written. If this setting is set to an empty string then no
    /// diagnostic information will be written.
    /// Update : This setting will now be interpreted as an absolute path if it contains the character 
    ///          sequence ":\"
    /// 
    /// Default value = 'CastleWindsor.dump.log'
    /// ----------------------------------------------------------------------------------------------------
    /// ConventionBasedInstallerNamespacePrefixes
    /// 
    /// A list of strings. If this setting is left empty then all classes defined in assemblies in the same
    /// folder as the main assembly will be considered for convention-based registration unless they are 
    /// defined in namespaces that begin 'Castle' or 'System'. If any values are added to the list then
    /// classes will only be considered for convention-based registration if they are in a namespace whose
    /// name begins with at least one of the strings in the list.
    /// 
    /// Default value = Empty List
    /// ----------------------------------------------------------------------------------------------------
    /// InstallerAssemblyFilterMask
    /// 
    /// The pattern that is applied to assemblies in the same folder as the main assembly to identify those
    /// assemblies whose classes should be considered for convention-based registration.
    /// 
    /// Default value = '*'
    /// ----------------------------------------------------------------------------------------------------
    /// ClearLogFile
    /// 
    /// true = clear the log file before writing messages
    /// false = append messages to the log file
    /// 
    /// Default value = true
    /// ----------------------------------------------------------------------------------------------------
    /// DumpAllTypes
    /// 
    /// true = all types that were considered for registration will be listed in the log
    /// false = only register types will be included in the log
    /// 
    /// Default value = false
    /// ----------------------------------------------------------------------------------------------------
    /// </summary>
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
            ConventionBasedInstallerNamespacePrefixes = new List<string>();
            InstallerAssemblyFilterMask = "*";
            ClearLogFile = true;
            DumpAllTypes = false;
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
        /// If this property is true then the log file will be cleared before any new messages are written. If it is false then 
        /// messages will be appended.
        /// </summary>
        public static bool ClearLogFile { get; set; }

        /// <summary>
        /// If this property is true then the log file will contain a list of all types that were considered for registration. This is useful
        /// for debugging dependency injection issues. If this property is false then only registered types will be logged.
        /// </summary>
        public static bool DumpAllTypes { get; set; }

        /// <summary>
        /// Convention-based installer namespace prefix - Only classes in namespaces with this prefix will be wired by convention
        /// </summary>
        public static List<string> ConventionBasedInstallerNamespacePrefixes { get; set; }

        public static void DumpConfiguration()
        {
            ContainerReporter.AddDiagnosticMessage("Simple Configuration Properties");
            ContainerReporter.AddDiagnosticMessage(string.Format("CastleWindsorConfigurationFolderName={0}", CastleWindsorConfigurationFolderName));
            ContainerReporter.AddDiagnosticMessage(string.Format("CastleWindsorConfigurationFilePattern={0}", CastleWindsorConfigurationFilePattern));
            ContainerReporter.AddDiagnosticMessage(string.Format("CastleWindsorDumpFileName={0}", CastleWindsorDumpFileName));
            ContainerReporter.AddDiagnosticMessage(string.Format("InstallerAssemblyFilterMask={0}", InstallerAssemblyFilterMask));

            foreach (string n in ConventionBasedInstallerNamespacePrefixes)
            {
                ContainerReporter.AddDiagnosticMessage(string.Format("ConventionBasedInstallerNamespacePrefixes[]={0}", n));
            }
        }
    }
}
