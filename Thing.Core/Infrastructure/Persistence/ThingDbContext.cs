using System.Data.Entity;
using System.Diagnostics;
using SampleLibrary.Domain;

namespace SampleLibrary.Infrastructure.Persistence
{
    public class ThingDbContext : DbContext
    {
        public ThingDbContext()
        {
            Debug.WriteLine($"Create {nameof(ThingDbContext)}");
        }

        public DbSet<Thing> Things { get; set; }

        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine($"Dispose {nameof(ThingDbContext)}");

            base.Dispose(disposing);
        }
    }
}