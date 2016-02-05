using System;
using Thing.Core.Contracts;

namespace Thing.Core.Infrastructure
{
    public class DefaultSecurityPoint : ISecurityPoint
    {
        public bool CanDoWork(Domain.Thing thing)
        {
            if (thing == null) throw new ArgumentNullException(nameof(thing));

            return thing.Name != "secret";
        }
    }
}