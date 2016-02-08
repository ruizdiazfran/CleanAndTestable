using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Thing.Core.Contracts;

namespace Thing.Core.Infrastructure.Persistence
{
    public class ThingRepository : IThingRepository
    {
        private readonly ThingDbContext _dbContext;
        private readonly DbSet<Domain.Thing> _dataSet;

        public ThingRepository(ThingDbContext dbContext)
        {
            _dbContext = dbContext;
            Debug.WriteLine($"Create {nameof(ThingRepository)}");

            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            _dataSet = dbContext.Things;
        }

        public async Task<Domain.Thing> FindByNameAsync(string name)
        {
            return await _dataSet.FirstOrDefaultAsync(_ => _.Name == name).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Domain.Thing>> GetAllAsync()
        {
            return await _dataSet.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Domain.Thing> GetByIdAsync(string id)
        {
            var entity = await _dataSet.FindAsync(id).ConfigureAwait(false);

            if (entity == null)
            {
                throw new KeyNotFoundException(id);
            }

            return entity;
        }

        public Task Add(Domain.Thing entity)
        {
            _dataSet.Add(entity);
            return Task.CompletedTask;
        }

        public async Task Delete(string id)
        {
            var entity = await GetByIdAsync(id).ConfigureAwait(false);
            _dataSet.Remove(entity);
        }
    }
}