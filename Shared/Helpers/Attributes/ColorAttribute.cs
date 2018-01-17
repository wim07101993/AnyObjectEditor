namespace Shared.Helpers.Attributes
{
    public class ColorAttribute : AAttribute
    {
        public static readonly PictureAttribute Yes = new PictureAttribute(true);
        public static readonly PictureAttribute No = new PictureAttribute(false);
        public static readonly PictureAttribute Default = No;
        public const string NAME = "color";


        public bool IsColor { get; private set; }

        public override string Name => NAME;

        public override object Value
        {
            get => IsColor;
            protected set => IsColor = (bool)value;
        }

        public override object DefaultValue { get; } = true;


        public ColorAttribute()
        {
        }

        public ColorAttribute(bool color) : base(color)
        {
        }
    }
}
