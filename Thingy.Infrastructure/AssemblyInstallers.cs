using Castle.MicroKernel.Registration;
using Castle.Windsor.Installer;

namespace Thingy.Infrastructure
{
    internal static class AssemblyInstallers
    {
        /// <summary>
        /// Get Assembly Installers
        /// </summary>
        /// <returns>An IWindsorInstaller encapsulating the full set of assembly installers</returns>
        internal static IWindsorInstaller GetInstallers()
        {
            return FromAssembly.InDirectory(AssemblyFilters.GetFilter());
        }
    }
}
