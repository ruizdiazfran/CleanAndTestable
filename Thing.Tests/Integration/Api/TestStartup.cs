using System.Web.Http;
using Autofac;
using Thing.Api;
using Thing.Api.Infrastructure;

namespace Thing.Tests.Integration.Api
{
    public class TestStartup : Startup
    {
        protected override IContainer GetContainer()
        {
            DbUtil.SeedDbContext();

            return new CompositionRoot().GetRegistrations().Build();
        }

        protected override HttpConfiguration GetConfiguration()
        {
            var httpConfiguration = base.GetConfiguration();
            httpConfiguration.MessageHandlers.Add(new PreAuthenticatedUser());
            return httpConfiguration;
        }

        protected override void Init()
        {
            //  skip init
        }
    }
}