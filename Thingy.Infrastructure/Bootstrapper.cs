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
                    container = new WindsorContainer();
                    container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

                    // XML Configuration added first
                    // Fluent/by-convention components can have their parameters specified in the XML by
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
                    // Components registered by this assembly's by-convention installer will have the
                    // same name as the class that implements the service.
                    // THIS DOES NOT WORK IF THE FLUENT/BY-CONVENTION INSTALLERS ARE INSTALLED FIRST!
                    foreach (IWindsorInstaller configInstaller in ConfigurationInstallers.GetInstallers())
                    {
                        container.Install(configInstaller);
                    }

                    container.Install(AssemblyInstallers.GetInstallers());

                    ContainerReporter.Dump();
                }

                return container;
            }

        }

    }

}
