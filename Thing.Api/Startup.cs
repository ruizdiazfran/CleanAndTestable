using System.Web.Http;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Newtonsoft.Json.Serialization;
using Owin;
using SampleLibrary.Infrastructure;
using Thing.Api;
using Thing.Api.Infrastructure;

[assembly: OwinStartup(typeof (Startup))]

namespace Thing.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Initer.Initialize();

            GlobalConfiguration.Configure(WebApiRegister.Register);

            var container = CompositionRoot.Create();

            var configuration = GlobalConfiguration.Configuration;

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacWebApi(configuration);

            app.UseWebApi(configuration);
        }
    }
}