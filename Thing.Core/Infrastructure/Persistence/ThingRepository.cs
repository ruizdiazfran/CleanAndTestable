using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading.Tasks;
using SampleLibrary.Contracts;
using SampleLibrary.Domain;

namespace SampleLibrary.Infrastructure.Persistence
{
    public class ThingRepository : IThingRepository
    {
        private readonly DbSet<Thing> _dataSet;

        public ThingRepository(ThingDbContext dbContext)
        {
            Debug.WriteLine($"Create {nameof(ThingRepository)}");

            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            _dataSet = dbContext.Things;
        }

        public async Task<Thing> FindByNameAsync(string name)
        {
            return await _dataSet.FirstOrDefaultAsync(_ => _.Name == name).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Thing>> GetAllAsync()
        {
            return await _dataSet.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Thing> GetByIdAsync(string id)
        {
            return await _dataSet.SingleAsync(_ => _.Id == id).ConfigureAwait(false);
        }

        public void Add(Thing entity)
        {
            _dataSet.Add(entity);
        }
    }
}