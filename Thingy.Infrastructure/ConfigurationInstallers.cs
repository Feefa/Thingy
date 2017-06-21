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
        /// <returns>An IWindsorInstaller encapsulating the full set of configuration-based installers</returns>
        internal static IEnumerable<IWindsorInstaller> GetInstallers()
        {
            IList<IWindsorInstaller> configInstallers = new List<IWindsorInstaller>();
            if (Directory.Exists(InfrastructureConfiguration.XmlConfigDirectory))
            {
                foreach (string fileName in Directory.GetFiles(InfrastructureConfiguration.XmlConfigDirectory, InfrastructureConfiguration.XmlConfigPattern))
                {
                    configInstallers.Add(Configuration.FromXmlFile(fileName));
                };
            }

            return configInstallers;
        }
    }
}
