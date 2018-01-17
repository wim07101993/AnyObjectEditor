namespace Shared.Helpers.Attributes
{
    public class TitleAttribute : AAttribute
    {
        public static readonly TitleAttribute Yes = new TitleAttribute(true);
        public static readonly TitleAttribute No = new TitleAttribute(false);
        public static readonly TitleAttribute Default = No;
        public const string NAME = "title";


        public bool IsTitle { get; set; } = true;

        public override string Name => NAME;
        public override object Value
        {
            get => IsTitle;
            protected set => IsTitle = (bool)value;
        }

        public override object DefaultValue { get; } = true;


        public TitleAttribute()
        {
        }

        public TitleAttribute(bool title):base(title)
        {
        }
    }
}