using System;
using System.Threading.Tasks;

namespace SampleLibrary.Contracts
{
    public interface IUnitOfWork
    {
        IThingRepository GetThingRepository();
        Task StartAsync();
        Task CommitAsync(Exception exception = null);
    }
}