using System;
using System.Net.Http;
using Autofac;
using Microsoft.Owin.Builder;
using Thing.Api;
using Thing.Api.Infrastructure;
using Thing.Core.Infrastructure.Persistence;

namespace Thing.Tests.Integration.Api
{
    public class TestStartup : Startup
    {
        protected override IContainer GetContainer()
        {
            //  reseed each case
            using (var db = DbUtil.CreateDbContext())
            {
                new DefaultDbInitializer().InitializeDatabase(db);
            }

            return new CompositionRoot().GetRegistrations().Build();
        }

        protected override void Init()
        {
            //  skip init
        }

        public static HttpClient CreateHttpClient()
        {
            var app = new AppBuilder();
            new TestStartup().Configuration(app);
            var handler = new OwinHttpMessageHandler(app.Build());
            return new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost")
            };
        }
    }
}