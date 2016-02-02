using System;
using System.Threading.Tasks;

namespace SampleLibrary.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IThingRepository GetThingRepository();

        Task CommitAsync(Exception exception);
    }
}