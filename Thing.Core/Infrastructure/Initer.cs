using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Thing.Core.Infrastructure.Mapper;
using Thing.Core.Infrastructure.Persistence;

namespace Thing.Core.Infrastructure
{
    public class Initer
    {
        private static MyInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public static void Initialize()
        {
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        public static async Task<bool> HasCompleted()
        {
            if (_initializer == null)
            {
                return false;
            }

            return await _initializer.IsInitialized;
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class MyInitializer
        {
            public MyInitializer()
            {
                IsInitialized = Task.Factory.StartNew(async () => await InitAsync()).Unwrap();
            }

            public Task<bool> IsInitialized { get; }


            private static async Task<bool> InitAsync()
            {
                try
                {
                    AutoMapperBootstrapper.Initialize();
                    await InitializeDbAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private static Task InitializeDbAsync()
            {
                Database.SetInitializer(new DefaultDbInitializer());

                return Task.CompletedTask;
            }
        }
    }
}