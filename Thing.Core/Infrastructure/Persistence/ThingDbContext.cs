using System.Data.Entity;
using SampleLibrary.Domain;

namespace SampleLibrary.Infrastructure.Persistence
{
    public class ThingDbContext : DbContext
    {
        public DbSet<Thing> Things { get; set; }
    }
}