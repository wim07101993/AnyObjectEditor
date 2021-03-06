﻿namespace Shared.Helpers.Attributes
{
    public class PictureAttribute : AAttribute
    {
        public static readonly PictureAttribute Yes = new PictureAttribute(true);
        public static readonly PictureAttribute No = new PictureAttribute(false);
        public static readonly PictureAttribute Default = No;
        public const string NAME = "picture";


        public bool IsPicture { get; private set; }

        public override string Name => NAME;

        public override object Value
        {
            get => IsPicture;
            protected set => IsPicture = (bool) value;
        }

        public override object DefaultValue { get; } = true;


        public PictureAttribute()
        {
        }

        public PictureAttribute(bool picture) : base(picture)
        {
        }
    }
}