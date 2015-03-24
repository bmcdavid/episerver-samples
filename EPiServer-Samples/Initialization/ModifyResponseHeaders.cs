using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using System;
using System.Web;

namespace EPiServerSamples.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ModifyResponseHeaders : IInitializableHttpModule
    {
        public void Initialize(InitializationEngine context)
        {

        }

        public void Preload(string[] parameters)
        {

        }

        public void Uninitialize(InitializationEngine context)
        {

        }

        public void InitializeHttpEvents(HttpApplication application)
        {
            application.PostReleaseRequestState += application_PostReleaseRequestState;
        }

        private void application_PostReleaseRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-AspNetMvc-Version");
            context.Response.Headers.Remove("X-AspNet-Version");
        }
    }
}