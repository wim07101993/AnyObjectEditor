using System;

namespace Shared.Helpers.Attributes
{
    public abstract class AAttribute : Attribute, IAttribute
    {
        public abstract string Name { get; }
        public abstract object Value { get; protected set; }
        public abstract object DefaultValue { get; }


        protected AAttribute()
        {
            Value = DefaultValue;
        }

        protected AAttribute(object value)
        {
            Value = value;
        }


        public override bool Equals(object obj)
            => obj == this || obj is IAttribute other && other.Value == Value;

        public override int GetHashCode()
            => Value?.GetHashCode() ?? -1;
    }
}