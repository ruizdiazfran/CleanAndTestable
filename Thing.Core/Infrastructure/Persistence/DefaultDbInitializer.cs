using System.Data.Entity;
using System.Diagnostics;

namespace Thing.Core.Infrastructure.Persistence
{
    public class DefaultDbInitializer : DropCreateDatabaseAlways<ThingDbContext>
    {
        protected override void Seed(ThingDbContext context)
        {
            Debug.WriteLine($"Seed {nameof(ThingDbContext)} {context.Identifier}");
            context.Things.Add(new Domain.Thing("my-first", "one").SetAddress("Via Morimondo", "20100"));
            context.Things.Add(new Domain.Thing("my-second", "two").SetAddress("Via Barona", "20100"));
            context.Things.Add(new Domain.Thing("my-thirdy", "three").SetAddress("Via Watts", "20100"));
            context.Things.Add(new Domain.Thing("my-fourthy", "secret").SetAddress("Corso Magenta", "20100"));
            context.SaveChanges();
        }
    }
}