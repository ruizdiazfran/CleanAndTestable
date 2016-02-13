using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using Thing.Api;
using Thing.Api.Infrastructure;
using Thing.Core.Infrastructure;

[assembly: OwinStartup(typeof (Startup))]

namespace Thing.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Init();

            var configuration = GetConfiguration(app);

            WebApiRegister.Register(configuration);

            var container = GetContainer(app);

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacWebApi(configuration);
            app.UseAutofacMiddleware(container);
            app.UseWebApi(configuration);
        }

        protected virtual void Init()
        {
            Initer.Initialize();
        }

        protected virtual HttpConfiguration GetConfiguration(IAppBuilder app)
        {
            return new HttpConfiguration();
        }

        protected virtual IContainer GetContainer(IAppBuilder app)
        {
            return CompositionRoot.Container;
        }
    }
}