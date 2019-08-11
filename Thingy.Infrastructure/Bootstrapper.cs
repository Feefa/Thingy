using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace Thingy.Infrastructure
{
    /// <summary>
    /// Bootstrapper - Provides a Castle Windsor container
    /// </summary>
    public static class Bootstrapper
    {
        static Bootstrapper()
        {
            Lock = new object();
        }

        /// <summary>
        /// container - An internal reference to the Castle Windsor container
        /// </summary>
        private static IWindsorContainer container = null;

        /// <summary>
        /// Gets the Castle Windsor container, creating and intialising it if it does not already exist
        /// </summary>
        public static IWindsorContainer Container
        {
            get
            {
                if (container == null)
                {
                    InfrastructureConfiguration.DumpConfiguration();
                    DerivedInfrastructureConfiguration.DumpConfiguration();
                    container = new WindsorContainer();
                    container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
                    // XML Configuration added first
                    // Fluent/convention-based components can have their parameters specified in the XML by
                    // simply including the id="" attribute in the <component> tag and no other attributes,
                    // and the <parameters> collection in the body of the component. For example.
                    // <components>
                    //   <components id="Example">
                    //     <parameters>
                    //       <ExampleProperty>value</ExampleProperty>
                    //     </parameters>
                    //   </components>
                    // </components>
                    // The component should be .Named() to match the id above in fluent registration.
                    // Components registered by this assembly's convention-based installer will have the
                    // same name as the class that implements the service.
                    // THIS DOES NOT WORK IF THE FLUENT/CONVENTION-BASED INSTALLERS ARE INSTALLED FIRST!
                    foreach (IWindsorInstaller configInstaller in ConfigurationInstallers.GetInstallers())
                    {
                        container.Install(configInstaller);
                    }
                    // Registration by Installers, including the convention-based installer
                    container.Install(AssemblyInstallers.GetInstallers());

                    ContainerReporter.Dump();
                }

                return container;
            }

        }

        /// <summary>
        /// A lock object for single threading
        /// </summary>
        public static object Lock { get; }

        /// <summary>
        /// Returns true if we have already initialized the Windsor Container
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
                return container != null;
            }
        }
    }

}
