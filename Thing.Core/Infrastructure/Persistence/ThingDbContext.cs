using System;
using System.Data.Entity;
using System.Diagnostics;

namespace Thing.Core.Infrastructure.Persistence
{
    public class ThingDbContext : DbContext
    {
        public readonly Guid Identifier = Guid.NewGuid();

        public ThingDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Debug.WriteLine($"Create {nameof(ThingDbContext)} {Identifier} {nameOrConnectionString}");
            Configure();
        }

        public ThingDbContext()
        {
            Debug.WriteLine($"Create {nameof(ThingDbContext)} {Identifier}");
            Configure();
        }

        public DbSet<Domain.Thing> Things { get; set; }

        private void Configure()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine($"Dispose {nameof(ThingDbContext)} {Identifier} ");

            base.Dispose(disposing);
        }
    }
}