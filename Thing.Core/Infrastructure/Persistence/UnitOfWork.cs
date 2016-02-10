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

        public Task StartAsync()
        {
            Debug.WriteLine($"  Start Transaction {_dbContext.Identifier}");
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
                    Debug.WriteLine($"  Rollback with Skip Transaction {_dbContext.Identifier}");
                    return;
                }

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                Debug.WriteLine($"  Commit Transaction {_dbContext.Identifier}");

                _dbTransaction?.Commit();
            }
            catch (Exception)
            {
                if (_dbTransaction?.UnderlyingTransaction.Connection != null)
                {
                    _dbTransaction?.Rollback();
                    Debug.WriteLine($"  Rollback Transaction {_dbContext.Identifier}");
                }

                throw;
            }
            finally
            {
                if (_dbTransaction != null)
                {
                    _dbTransaction.Dispose();
                    _dbTransaction = null;
                    Debug.WriteLine($"  Dispose Transaction {_dbContext.Identifier}");
                }
            }
        }
    }
}