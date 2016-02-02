using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using MediatR;
using SampleLibrary.Contracts;
using SampleLibrary.Infrastructure;
using SampleLibrary.Infrastructure.Persistence;
using SampleLibrary.Query;

namespace Thing.Api
{
    public class CompositionRoot
    {
        public IContainer Create(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //Security
            builder.Register<ISecurityPoint>(_ => new DefaultSecurityPoint()).SingleInstance();

            //  UnitOfWork
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.Register(_ => _.Resolve<IUnitOfWork>().GetThingRepository())
                .InstancePerLifetimeScope();

            builder.RegisterType<ThingDbContext>().AsSelf();

            //  MediatoR
            builder.RegisterAssemblyTypes(typeof (IMediator).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof (ThingQuery).Assembly).AsImplementedInterfaces();
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>) c.Resolve(typeof (IEnumerable<>).MakeGenericType(t));
            });

            var container = builder.Build();

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }
}