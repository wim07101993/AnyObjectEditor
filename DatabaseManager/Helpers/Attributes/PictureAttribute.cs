using System;

namespace DatabaseManager.Helpers.Attributes
{
    public class PictureAttribute : Attribute
    {
        public static readonly PictureAttribute Yes = new PictureAttribute(true);
        public static readonly PictureAttribute No = new PictureAttribute(false);
        public static readonly PictureAttribute Default = No;

        public bool Picture { get; } = true;

        public PictureAttribute()
        {
        }

        public PictureAttribute(bool picture)
        {
            Picture = picture;
        }

        public override bool Equals(object obj)
            => obj == this || obj is PictureAttribute other && other.Picture == Picture;

        public override int GetHashCode()
            => Picture.GetHashCode();
    }
}