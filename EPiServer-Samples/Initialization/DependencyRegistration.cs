using EPiServer.Framework;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using EPiServerSamples.Rendering;

namespace EPiServerSamples.Initialization
{
    // allows EPiServer to fully register itself before we override ContentAreaRenderer
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DependencyRegistration : IConfigurableModule
    {
        public void ConfigureContainer(EPiServer.ServiceLocation.ServiceConfigurationContext context)
        {
            // Since the Container is using StructureMap, auto registrations and type scanning can also occur.
            // Review http://docs.structuremap.net/ScanningAssemblies.htm for more information.

            context.Container.Configure
            (
                configure =>
                {
                    configure.For<ContentAreaRenderer>().Use<CustomContentAreaRenderer>();                
                }
            );
        }

        public void Initialize(EPiServer.Framework.Initialization.InitializationEngine context)
        {
            
        }

        public void Uninitialize(EPiServer.Framework.Initialization.InitializationEngine context)
        {
            
        }

        public void Preload(string[] parameters)
        {
            
        }
    }
}