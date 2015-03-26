using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace EPiServerSamples.Initialization
{
    //[InitializableModule] // attribute needed before the module is loaded at startup
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))] // required when setting display options
    public class DisplayOptionsRegistration : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            // Register Display Options that ContentAreaItems can utilize to change sizing
            var options = ServiceLocator.Current.GetInstance<DisplayOptions>();

            options
                .Add("span12", "Full", "span12", "", "")
                .Add("span8", "Two-Thirds", "span8", "", "")
                .Add("span6", "Half", "span6", "", "")
                .Add("span4", "One-Third", "span4", "", "");
        }

        public void Preload(string[] parameters)
        {

        }

        public void Uninitialize(InitializationEngine context)
        {

        }
    }
}