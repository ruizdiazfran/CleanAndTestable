using System;
using SampleLibrary.Contracts;
using SampleLibrary.Domain;

namespace SampleLibrary.Infrastructure
{
    public class DefaultSecurityPoint : ISecurityPoint
    {
        public bool CanDoWork(Thing thing)
        {
            if (thing == null) throw new ArgumentNullException(nameof(thing));

            return thing.Name != "secret";
        }
    }
}