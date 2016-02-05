using System;
using System.Data.Entity;
using System.Diagnostics;

namespace Thing.Core.Infrastructure.Persistence
{
    public class ThingDbContext : DbContext
    {
        public readonly Guid Identifier = new Guid();

        public ThingDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Debug.WriteLine($"Create {nameof(ThingDbContext)}");
        }

        public ThingDbContext()
        {
            Debug.WriteLine($"Create {nameof(ThingDbContext)}");
        }

        public DbSet<Domain.Thing> Things { get; set; }

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine($"Dispose {nameof(ThingDbContext)}");

            base.Dispose(disposing);
        }
    }
}