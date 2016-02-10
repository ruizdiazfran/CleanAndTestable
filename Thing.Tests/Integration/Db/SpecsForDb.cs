using System;
using System.Data.Entity;
using Thing.Core.Infrastructure.Persistence;

namespace Thing.Tests.Integration.Db
{
    public abstract class SpecsForDb
    {
        protected void Persist(Action action)
        {
            var dbContext = ContainerLocal.Resolve<ThingDbContext>();
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
            var dbContext = ContainerLocal.Resolve<ThingDbContext>();
            action(dbContext);
        }
    }
}