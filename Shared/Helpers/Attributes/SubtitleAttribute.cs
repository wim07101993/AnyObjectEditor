using System;

namespace Shared.Helpers.Attributes
{
    public class SubtitleAttribute : Attribute
    {
        public static readonly SubtitleAttribute Yes = new SubtitleAttribute(true);
        public static readonly SubtitleAttribute No = new SubtitleAttribute(false);
        public static readonly SubtitleAttribute Default = No;

        public bool Subtitle { get; } = true;

        public SubtitleAttribute()
        {
        }

        public SubtitleAttribute(bool subtitle)
        {
            Subtitle = subtitle;
        }

        public override bool Equals(object obj)
            => obj == this || obj is SubtitleAttribute other && other.Subtitle == Subtitle;

        public override int GetHashCode()
            => Subtitle.GetHashCode();
    }
}