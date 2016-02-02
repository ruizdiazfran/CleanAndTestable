using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleLibrary.Domain
{
    [Table("ThingCommand")]
    public class Thing : IAggregateRoot
    {
        protected Thing()
        {
        }

        public Thing(string id, string name)
        {
            Id = id;
            Name = name;
            Address = Address.Empty;
        }

        public string Name { get; private set; }
        public Address Address { get; private set; }

        [Key]
        public string Id { get; protected set; }

        public Thing SetAddress(string line, string zip)
        {
            Address = new Address(line, zip);
            return this;
        }

        public bool Equals(Thing entity)
        {
            if (ReferenceEquals(this, entity)) return true;
            if (ReferenceEquals(null, entity)) return false;
            return Id.Equals(entity.Id);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as Thing);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode()*907 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}