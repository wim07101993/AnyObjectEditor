namespace Shared.Helpers.Attributes
{
    public class DescriptionAttribute : AAttribute
    {
        public static readonly DescriptionAttribute Yes = new DescriptionAttribute(true);
        public static readonly DescriptionAttribute No = new DescriptionAttribute(false);
        public static readonly DescriptionAttribute Default = No;
        public const string NAME = "description";


        public bool Description { get; private set; }

        public override string Name => NAME;

        public override object Value
        {
            get => Description;
            protected set => Description = (bool) value;
        }

        public override object DefaultValue { get; } = true;


        public DescriptionAttribute()
        {
        }

        public DescriptionAttribute(bool description) : base(description)
        {
        }
    }
}