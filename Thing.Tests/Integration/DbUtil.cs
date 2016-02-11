using System;
using Thing.Core.Infrastructure.Persistence;

namespace Thing.Tests.Integration
{
    public class DbUtil
    {
        internal static ThingDbContext CreateDbContext()
        {
            var dbContext = new ThingDbContext();
            dbContext.Database.Log = Console.WriteLine;
            return dbContext;
        }

        internal static void SeedDbContext()
        {
            using (var db = CreateDbContext())
            {
                new DefaultDbInitializer().InitializeDatabase(db);
            }
        }
    }
}