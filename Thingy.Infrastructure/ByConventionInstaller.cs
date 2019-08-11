using System;
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
        /// - Is in an allowed namespace
        /// - Implements an interface the is also in an allowed namespace
        /// - Does not implement IWindsorInstaller
        /// - Is not marked with the DoNotInstallByConvention attribute
        /// - Contains the unprefixed name of the interface in its own name (DefaultInterfaces)
        /// </summary>
        /// <param name="container">An IWindsorContainer implementation</param>
        /// <param name="store">An IConfigurationStore implementation</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromAssemblyInDirectory(AssemblyFilters.GetFilter())
                    .Where(t => ContainerReporter.AddType(t) &&
                           IsInAllowedNameNamespace(t) &&
                           t.GetInterfaces().Any(i => IsInAllowedNameNamespace(i)) &&
                           !t.GetInterfaces().Contains(typeof(IWindsorInstaller)) &&
                           !t.GetCustomAttributes(false).Where(a => a.GetType() == typeof(DoNotInstallByConventionAttribute)).Any())
                    .WithServiceDefaultInterfaces()
                    .Configure(c => c.Named(c.Implementation.Name)));
        }

        /// <summary>
        /// Check if the type is within an allowed namespace. The default is that all namespaces are allowed except for those starting
        /// System or Castle. If any NamespacePrefixes have been added to configuration then only those namespaces will be allowed.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>True if the type is defined in an allowed namespace, false otherwise.</returns>
        private static bool IsInAllowedNameNamespace(Type type)
        {
            if (DerivedInfrastructureConfiguration.NamespacePrefixes.Count == 0)
            {
                return !type.Namespace.StartsWith("System") && !type.Namespace.StartsWith("Castle");
            }
            else
            {
                return DerivedInfrastructureConfiguration.NamespacePrefixes.Any(n => type.Namespace.StartsWith(n));
            }
        }
    }
}
