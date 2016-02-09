using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Thing.Core.Infrastructure.Persistence;
using Thing.Tests.Utils;

namespace Thing.Tests.Integration.Db
{
    public abstract class SpecsForDb
    {
        protected void Persist(Action action)
        {
            var dbContext = DbLocal.GetTypedDbContext<ThingDbContext>();
            var tx = dbContext.Database.BeginTransaction();

            try
            {
                action();
                dbContext.SaveChanges();
                tx.Commit();
            }
            catch (Exception)
            {
                tx.Rollback();
                throw;
            }
            finally
            {
                UndoingChangesDbContextLevel(dbContext);
            }
        }

        private static void UndoingChangesDbContextLevel(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        protected void Do(Action<ThingDbContext> action)
        {
            action(DbLocal.GetTypedDbContext<ThingDbContext>());
        }
    }
}