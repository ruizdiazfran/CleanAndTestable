using System;
using Thing.Core.Infrastructure.Persistence;
using Thing.Tests.Integration.Utils;

namespace Thing.Tests.Integration.Db
{
    public abstract class SpecsForDb
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
            action(DbLocal.GetTypedDbContext<ThingDbContext>());
        }
    }
}