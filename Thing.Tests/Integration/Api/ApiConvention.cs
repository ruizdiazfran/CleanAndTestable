using System;
using System.Web.Http;
using Autofac;

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
            builder.Register(c => new HttpConfiguration());
            builder.Register(c => new HttpConfiguration().ToHttpClient());
            return builder.Build();
        }

        protected override object CustomCtorFactory(Type t)
        {
            return _container.Resolve(t);
        }
    }
}