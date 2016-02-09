using System;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading.Tasks;
using Thing.Core.Contracts;

namespace Thing.Core.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ThingDbContext _dbContext;
        private DbContextTransaction _dbTransaction;

        public UnitOfWork(ThingDbContext dbContext)
        {
            Debug.WriteLine($"Create {nameof(UnitOfWork)} {dbContext.Identifier}");
            _dbContext = dbContext;
        }

        public IThingRepository GetThingRepository()
        {
            return new ThingRepository(_dbContext);
        }

        public Task StartAsync()
        {
            _dbTransaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            return Task.CompletedTask;
        }

        public async Task CommitAsync(Exception exception = null)
        {
            try
            {
                if (_dbTransaction != null && exception != null)
                {
                    _dbTransaction.Rollback();
                    return;
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                _dbTransaction?.Commit();
            }
            catch (Exception)
            {
                if (_dbTransaction?.UnderlyingTransaction.Connection != null)
                {
                    _dbTransaction?.Rollback();
                }

                throw;
            }
            finally
            {
                if (_dbTransaction != null)
                {
                    _dbTransaction.Dispose();
                    _dbTransaction = null;
                }
            }
        }
    }
}