using System.Collections.Generic;
using System.Threading.Tasks;
using SampleLibrary.Domain;

namespace SampleLibrary.Contracts
{
    public interface IThingRepository
    {
        Task<Thing> GetByIdAsync(string id);

        Task<Thing> FindByNameAsync(string name);

        Task<IEnumerable<Thing>> GetAllAsync();

        void Add(Thing entity);
    }
}