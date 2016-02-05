using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading.Tasks;
using Thing.Core.Contracts;

namespace Thing.Core.Infrastructure.Persistence
{
    public class ThingRepository : IThingRepository
    {
        private readonly DbSet<Domain.Thing> _dataSet;

        public ThingRepository(ThingDbContext dbContext)
        {
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
            return await _dataSet.SingleAsync(_ => _.Id == id).ConfigureAwait(false);
        }

        public void Add(Domain.Thing entity)
        {
            _dataSet.Add(entity);
        }
    }
}