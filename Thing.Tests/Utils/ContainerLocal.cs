using System;
using System.Diagnostics;
using System.Threading;
using Autofac;
using Thing.Api.Infrastructure;

namespace Thing.Tests.Utils
{
    public static class ContainerLocal
    {
        private static ThreadLocal<ILifetimeScope> _instance;

        private static ILifetimeScope ValueFactory()
        {
            Debug.WriteLine($"Create nested container");
            return CompositionRoot.Container.BeginLifetimeScope();
        }

        public static T Resolve<T>()
        {
            return _instance.Value.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return _instance.Value.Resolve(type);
        }

        public static bool IsRegistered<T>()
        {
            return _instance.Value.IsRegistered<T>();
        }

        public static void Begin()
        {
            if (_instance != null)
            {
                return;
            }

            _instance = new ThreadLocal<ILifetimeScope>(ValueFactory);

            //  create nested container
            if (_instance.Value == null)
            {
                throw new InvalidOperationException("Container nested is not created");
            }
        }

        public static void End()
        {
            Debug.WriteLine($"Dispose nested container");

            _instance?.Value.Dispose();

            _instance = null;
        }
    }
}