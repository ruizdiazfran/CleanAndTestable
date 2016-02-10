using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using Autofac.Integration.WebApi;
using FluentValidation;
using MediatR;
using Thing.Core.Command;
using Thing.Core.Contracts;
using Thing.Core.Infrastructure;
using Thing.Core.Infrastructure.Persistence;
using Thing.Core.Infrastructure.Pipeline;

namespace Thing.Api.Infrastructure
{
    public class CompositionRoot
    {
        private static readonly Lazy<IContainer> Bootstrapper =
            new Lazy<IContainer>(() => new CompositionRoot().GetRegistrations().Build(), true);

        public static IContainer Container => Bootstrapper.Value;

        public ContainerBuilder GetRegistrations()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //Security
            builder.Register<ISecurityPoint>(_ => new DefaultSecurityPoint()).SingleInstance();

            //  Data & Co.
            builder.Register(_ => new UnitOfWork(_.Resolve<ThingDbContext>()))
                .InstancePerLifetimeScope().AsImplementedInterfaces();

            builder.Register(_ => new ThingRepository(_.Resolve<ThingDbContext>()))
                .InstancePerLifetimeScope().AsImplementedInterfaces();

            builder.RegisterType<ThingDbContext>()
                .AsSelf().InstancePerLifetimeScope();

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

            return builder;
        }

        private static void RegisterValidators(ContainerBuilder builder)
        {
            AssemblyScanner.FindValidatorsInAssembly(typeof (ThingCommand).Assembly)
                .ForEach(
                    result =>
                    {
                        builder.RegisterType(result.ValidatorType).As(result.InterfaceType).InstancePerLifetimeScope();
                    });
        }
    }
}