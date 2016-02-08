﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thing.Core.Contracts
{
    public interface IThingRepository
    {
        Task<Domain.Thing> GetByIdAsync(string id);

        Task<Domain.Thing> FindByNameAsync(string name);

        Task<IEnumerable<Domain.Thing>> GetAllAsync();

        Task Add(Domain.Thing entity);

        Task Delete(string id);
    }
}