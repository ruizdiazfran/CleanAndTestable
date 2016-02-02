using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;

namespace Thing.Api
{
    internal static class AutofacBuilderExtensions
    {
        public static void RegisterHandlers(this ContainerBuilder builder, Assembly assembly, Type handlerType,
            params Type[] decorators)
        {
            RegisterHandlers(builder, assembly, handlerType);
            for (var i = 0; i < decorators.Length; i++)
            {
                RegisterGenericDecorator(
                    builder,
                    decorators[i],
                    handlerType, i == 0 ? handlerType : decorators[i - 1], i == decorators.Length - 1);
            }
        }

        private static void RegisterHandlers(ContainerBuilder builder, Assembly assembly, Type handlerType)
        {
            var registrationBuilder = builder.RegisterAssemblyTypes(assembly)
                .As(t => t.GetInterfaces()
                    .Where(v => v.IsClosedTypeOf(handlerType))
                    .Select(v => new KeyedService(handlerType.Name, v)));

            registrationBuilder.InstancePerLifetimeScope();
        }

        private static void RegisterGenericDecorator(ContainerBuilder builder, Type decoratorType,
            Type decoratedServiceType, Type fromKeyType, bool isTheLast)
        {
            var result = builder.RegisterGenericDecorator(decoratorType, decoratedServiceType, fromKeyType.Name);
            if (!isTheLast)
            {
                result.Keyed(decoratorType.Name, decoratedServiceType);
            }
        }
    }
}