using System.Web.Http;
using Autofac;
using Owin;
using Thing.Api;
using Thing.Api.Infrastructure;

namespace Thing.Tests.Integration.Api
{
    public class TestStartup : Startup
    {
        private readonly HttpConfiguration _configuration;

        public TestStartup(HttpConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override IContainer GetContainer(IAppBuilder app)
        {
            DbUtil.SeedDbContext();

            return new CompositionRoot().GetRegistrations().Build();
        }

        protected override HttpConfiguration GetConfiguration(IAppBuilder app)
        {
            _configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            return _configuration;
        }

        protected override void Init()
        {
            //  skip init
        }
    }
}