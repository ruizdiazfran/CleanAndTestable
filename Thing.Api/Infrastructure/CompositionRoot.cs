using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using Autofac.Integration.WebApi;
using FluentValidation;
using MediatR;
using SampleLibrary.Command;
using SampleLibrary.Contracts;
using SampleLibrary.Infrastructure;
using SampleLibrary.Infrastructure.Persistence;
using SampleLibrary.Infrastructure.Pipeline;

namespace Thing.Api.Infrastructure
{
    public class CompositionRoot
    {
        public static IContainer Create()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //Security
            builder.Register<ISecurityPoint>(_ => new DefaultSecurityPoint()).SingleInstance();

            //  UnitOfWork
            builder.Register(_ => new UnitOfWork(_.Resolve<ThingDbContext>()))
                .InstancePerLifetimeScope().AsImplementedInterfaces();

            builder.Register(_ => _.Resolve<IUnitOfWork>().GetThingRepository())
                .InstancePerLifetimeScope();

            builder.RegisterType<ThingDbContext>()
                .AsSelf();

            //  Validators
            RegisterValidators(builder);

            //  MediatR
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof (IMediator).Assembly)
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof (ThingCommand).Assembly)
                .As(o => o.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof (IAsyncRequestHandler<,>)))
                    .Select(i => new KeyedService("async-handlers", i)));
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
            builder.RegisterGenericDecorator(typeof (AsyncValidationRequestHandler<,>), typeof (IAsyncRequestHandler<,>),
                "async-handlers"); // The outermost decorator should not have a toKey

            return builder.Build();
        }

        private static void RegisterValidators(ContainerBuilder builder)
        {
            AssemblyScanner.FindValidatorsInAssembly(typeof (ThingCommand).Assembly)
                .ForEach(result =>
                    {
                        builder.RegisterType(result.ValidatorType).As(result.InterfaceType).InstancePerLifetimeScope();
                    });
        }
    }
}