using Castle.MicroKernel.Registration;

namespace Thingy.Infrastructure
{
    internal static class AssemblyFilters
    {
        /// <summary>
        /// Get Assembly Filter
        /// </summary>
        /// <returns>An AssemblyFilter that describes the set of assemblies that we are interested in.</returns>
        internal static AssemblyFilter GetFilter()
        {
            return new AssemblyFilter(DerivedInfrastructureConfiguration.AssemblyDirectory, DerivedInfrastructureConfiguration.AssemblyFilterMask);
        }
    }

}
