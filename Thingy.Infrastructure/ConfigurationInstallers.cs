using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor.Installer;
using System.IO;

namespace Thingy.Infrastructure
{
    internal static class ConfigurationInstallers
    {
        /// <summary>
        /// Get Configuration Installers
        /// </summary>
        /// <returns>An IEnumberable of IWindsorInstaller implementations, one for each XML config file</returns>
        internal static IEnumerable<IWindsorInstaller> GetInstallers()
        {
            IList<IWindsorInstaller> configInstallers = new List<IWindsorInstaller>();

            if (Directory.Exists(DerivedInfrastructureConfiguration.XmlConfigDirectory))
            {
                foreach (string fileName in Directory.GetFiles(DerivedInfrastructureConfiguration.XmlConfigDirectory, DerivedInfrastructureConfiguration.XmlConfigPattern))
                {
                    configInstallers.Add(Configuration.FromXmlFile(fileName));
                }
            }

            return configInstallers;
        }
    }
}
