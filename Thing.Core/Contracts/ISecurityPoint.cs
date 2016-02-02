using SampleLibrary.Domain;

namespace SampleLibrary.Contracts
{
    public interface ISecurityPoint
    {
        bool CanDoWork(Thing thing);
    }
}