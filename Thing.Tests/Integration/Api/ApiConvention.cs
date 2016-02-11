using System;
using System.Net.Http;
using Autofac;
using Microsoft.Owin.Builder;

namespace Thing.Tests.Integration.Api
{
    public class ApiConvention : FixieConventionBase
    {
        private readonly IContainer _container;

        public ApiConvention()
        {
            Classes.InTheSameNamespaceAs(typeof (ApiConvention));
            _container = BuildContainer();
        }

        protected IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            //  register tests
            builder.RegisterTests();
            builder.Register(BuildHandler);
            builder.Register(BuildHttpClient);
            return builder.Build();
        }

        private static HttpClient BuildHttpClient(IComponentContext arg)
        {
            return arg.Resolve<HttpMessageHandler>().ToHttpClient();
        }

        private static HttpMessageHandler BuildHandler(IComponentContext arg)
        {
            var app = new AppBuilder();
            new TestStartup().Configuration(app);
            var handler = new OwinHttpMessageHandler(app.Build());
            return handler;
        }

        protected override object CustomCtorFactory(Type t)
        {
            return _container.Resolve(t);
        }
    }
}