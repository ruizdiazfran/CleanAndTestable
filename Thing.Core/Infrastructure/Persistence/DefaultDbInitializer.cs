using System.Data.Entity;

namespace Thing.Core.Infrastructure.Persistence
{
    public class DefaultDbInitializer : DropCreateDatabaseAlways<ThingDbContext>
    {
        protected override void Seed(ThingDbContext context)
        {
            context.Things.Add(new Domain.Thing("my-first", "one").SetAddress("Via Morimondo", "20100"));
            context.Things.Add(new Domain.Thing("my-second", "two").SetAddress("Via Barona", "20100"));
            context.Things.Add(new Domain.Thing("my-thirdy", "three").SetAddress("Via Watts", "20100"));
            context.SaveChanges();
        }
    }
}