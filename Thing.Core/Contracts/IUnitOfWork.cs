using System;
using System.Threading.Tasks;

namespace Thing.Core.Contracts
{
    public interface IUnitOfWork
    {
        Task StartAsync();
        Task CommitAsync(Exception exception = null);
    }
}