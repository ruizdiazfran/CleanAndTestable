using System.Web.Http;
using Microsoft.Owin;
using Owin;
using SampleLibrary.Infrastructure;
using Thing.Api;

[assembly: OwinStartup(typeof (Startup))]

namespace Thing.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Initer.Initialize();

            var configuration = new HttpConfiguration();

            configuration.MapHttpAttributeRoutes();

            var container = new CompositionRoot().Create(configuration);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(configuration);
            
            app.UseWebApi(configuration);
        }
    }
}