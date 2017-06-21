using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Thingy.Infrastructure
{
    public class ByConventionInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Registers any classes in assemblies indicated by the AssemblyFilter that
        /// - Starts with a namespace prefix specified by InfrastructureConfiguration
        /// - Implements at least one interfaces
        /// - Does not implement IWindsorInstaller
        /// - Is not marked with the DoNotInstallByConvention attribute
        /// </summary>
        /// <param name="container">An IWindsorContainer implementation</param>
        /// <param name="store">An IConfigurationStore implementation</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromAssemblyInDirectory(AssemblyFilters.GetFilter())
                    .Where(p => ContainerReporter.AddType(p) &&
                           (InfrastructureConfiguration.NamespacePrefixes.Count == 0 ||
                            InfrastructureConfiguration.NamespacePrefixes.Any(n => p.Namespace.StartsWith(n))) &&
                           p.GetInterfaces().Any() &&
                           !p.GetInterfaces().Contains(typeof(IWindsorInstaller)) &&
                           !p.GetCustomAttributes(false).Where(a => a.GetType() == typeof(DoNotInstallByConventionAttribute)).Any())
                    .WithServiceDefaultInterfaces()
                    .Configure(c => c.Named(c.Implementation.Name)));
        }
    }
}
