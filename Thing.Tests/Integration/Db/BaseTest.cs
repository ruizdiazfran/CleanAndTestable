using System;
using Thing.Core.Infrastructure.Persistence;
using Thing.Tests.Integration.Utils;

namespace Thing.Tests.Integration.Db
{
    public abstract class BaseTest
    {
        protected void Tx(Action<ThingDbContext> action)
        {
            var dbContext = DbLocal.GetTypedDbContext<ThingDbContext>();
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

        protected void Check(Action<ThingDbContext> action)
        {
            var context = DbLocal.GetTypedDbContext<ThingDbContext>();
            action(context);
        }

        protected void Seed()
        {
            Tx(db => { db.Things.Add(new Core.Domain.Thing("my-first", "one").SetAddress("Via Morimondo", "20100")); });
        }
    }
}