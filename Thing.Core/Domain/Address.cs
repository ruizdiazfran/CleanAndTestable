using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleLibrary.Domain
{
    [ComplexType]
    public class Address : IValueObject
    {
        public static readonly Address Empty = new Address(string.Empty, string.Empty);

        protected Address()
        {
        }

        public Address(string line, string zip)
        {
            if (line == null) throw new InvalidOperationException();
            if (zip == null) throw new InvalidOperationException();
            Line = line;
            Zip = zip;
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public string Line { get; private set; }
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public string Zip { get; private set; }

        public override string ToString()
        {
            return $"Line:{Line} Zip:{Zip}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;

            var other = obj as Address;

            if (other == null) return false;

            return other.Line == Line && other.Zip == Zip;
        }

        public override int GetHashCode()
        {
            return Line.GetHashCode() | Zip.GetHashCode();
        }
    }
}