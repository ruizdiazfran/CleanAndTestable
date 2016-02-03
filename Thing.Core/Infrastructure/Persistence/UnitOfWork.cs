using System;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading.Tasks;
using SampleLibrary.Contracts;

namespace SampleLibrary.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ThingDbContext _dbContext;
        private readonly DbContextTransaction _dbTransaction;

        public UnitOfWork(ThingDbContext dbContext)
        {
            Debug.WriteLine($"Create {nameof(UnitOfWork)}");
            _dbContext = dbContext;
            _dbTransaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public IThingRepository GetThingRepository()
        {
            return new ThingRepository(_dbContext);
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
        }        
    }
}