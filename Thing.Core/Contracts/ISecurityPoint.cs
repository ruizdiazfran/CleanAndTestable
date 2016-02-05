namespace Thing.Core.Contracts
{
    public interface ISecurityPoint
    {
        bool CanDoWork(Domain.Thing thing);
    }
}