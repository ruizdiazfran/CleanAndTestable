using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading;
using Autofac;
using Thing.Api.Infrastructure;

namespace Thing.Tests.Integration
{
    public static class ContainerLocal
    {
        private static ThreadLocal<ILifetimeScope> _instance;
        private static readonly ThreadLocal<DbContext> Db = new ThreadLocal<DbContext>(DbUtil.CreateDbContext);

        private static ILifetimeScope ValueFactory()
        {
            Debug.WriteLine($"Create nested container");

            var builder = new CompositionRoot()
                .GetRegistrations()
                .RegisterTests();

            //  inject local DbContext
            DbUtil.SeedDbContext();
            builder.Register(_ => Db.Value);

            return builder.Build().BeginLifetimeScope();
        }

        public static T Resolve<T>()
        {
            return _instance.Value.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return _instance.Value.Resolve(type);
        }

        public static IDisposable Create()
        {
            _instance = new ThreadLocal<ILifetimeScope>(ValueFactory);

            return new DisposableAction(End);
        }

        private static void End()
        {
            Debug.WriteLine($"Dispose nested container");

            _instance?.Value.Dispose();

            _instance?.Dispose();

            _instance = null;
        }
    }

    internal sealed class DisposableAction : IDisposable
    {
        private Action _disposeAction;

        public DisposableAction(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            // Interlocked allows the continuation to be executed only once
            var continuation = Interlocked.Exchange(ref _disposeAction, null);

            continuation?.Invoke();
        }
    }
}