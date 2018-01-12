using System;

namespace Shared.Helpers.Attributes
{
    public class DescriptionAttribute : Attribute
    {
        public static readonly DescriptionAttribute Yes = new DescriptionAttribute(true);
        public static readonly DescriptionAttribute No = new DescriptionAttribute(false);
        public static readonly DescriptionAttribute Default = No;

        public bool Description { get; } = true;

        public DescriptionAttribute()
        {
        }

        public DescriptionAttribute(bool description)
        {
            Description = description;
        }

        public override bool Equals(object obj)
            => obj == this || obj is DescriptionAttribute other && other.Description == Description;

        public override int GetHashCode()
            => Description.GetHashCode();
    }
}