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

        internal static void Persist(Action<ThingDbContext> action)
        {
            using (var dbContext = new ThingDbContext())
            {
                dbContext.Database.Log = Console.WriteLine;

                var tx = dbContext.Database.BeginTransaction();

                try
                {
                    action(dbContext);
                    dbContext.SaveChanges();
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
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