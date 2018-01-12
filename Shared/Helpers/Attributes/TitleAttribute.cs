using System;

namespace Shared.Helpers.Attributes
{
    public class TitleAttribute : Attribute
    {
        public static readonly TitleAttribute Yes = new TitleAttribute(true);
        public static readonly TitleAttribute No = new TitleAttribute(false);
        public static readonly TitleAttribute Default = No;

        public bool Title { get; } = true;

        public TitleAttribute()
        {
        }

        public TitleAttribute(bool title)
        {
            Title = title;
        }

        public override bool Equals(object obj)
            => obj == this || obj is TitleAttribute other && other.Title == Title;

        public override int GetHashCode()
            => Title.GetHashCode();
    }
}