using System;

namespace Thing.Core.Contracts
{
    public class EntityNotFound : Exception
    {
        public EntityNotFound(string id) : base($"Entity not found {id}")
        {
            
        }
    }
}