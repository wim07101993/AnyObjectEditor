namespace Shared.Helpers.Attributes
{
    public class ImageAttribute : AAttribute
    {
        public static readonly PictureAttribute Yes = new PictureAttribute(true);
        public static readonly PictureAttribute No = new PictureAttribute(false);
        public static readonly PictureAttribute Default = No;
        public const string NAME = "image";


        public bool IsImage { get; private set; }

        public override string Name => NAME;

        public override object Value
        {
            get => IsImage;
            protected set => IsImage = (bool)value;
        }

        public override object DefaultValue { get; } = true;


        public ImageAttribute()
        {
        }

        public ImageAttribute(bool image) : base(image)
        {
        }
    }
}
