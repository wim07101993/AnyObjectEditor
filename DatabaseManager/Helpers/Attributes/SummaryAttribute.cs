using System;

namespace DatabaseManager.Helpers.Attributes
{
    public class SummaryAttribute : Attribute
    {
        public static readonly SummaryAttribute Yes = new SummaryAttribute(true);
        public static readonly SummaryAttribute No = new SummaryAttribute(false);
        public static readonly SummaryAttribute Default = No;

        public bool Summary { get; } = true;

        public SummaryAttribute()
        {
        }

        public SummaryAttribute(bool summary)
        {
            Summary = summary;
        }

        public override bool Equals(object obj)
            => obj == this || obj is SummaryAttribute other && other.Summary == Summary;

        public override int GetHashCode()
            => Summary.GetHashCode();
    }
}
}
