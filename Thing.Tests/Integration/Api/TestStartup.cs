using Autofac;
using Owin;
using Thing.Api;
using Thing.Api.Infrastructure;

namespace Thing.Tests.Integration.Api
{
    public class TestStartup : Startup
    {
        protected override IContainer GetContainer(IAppBuilder app)
        {
            DbUtil.SeedDbContext();

            return new CompositionRoot().GetRegistrations().Build();
        }

        protected override void Init()
        {
            //  skip init
        }
    }
}