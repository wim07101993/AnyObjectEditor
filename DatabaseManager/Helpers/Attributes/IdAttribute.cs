using System;

namespace DatabaseManager.Helpers.Attributes
{
    public class IdAttribute : Attribute
    {
        public static readonly IdAttribute Yes = new IdAttribute(true);
        public static readonly IdAttribute No = new IdAttribute(false);
        public static readonly IdAttribute Default = No;

        public bool Id { get; } = true;

        public IdAttribute()
        {
        }

        public IdAttribute(bool description)
        {
            Id = description;
        }

        public override bool Equals(object obj)
            => obj == this || obj is IdAttribute other && other.Id == Id;

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}