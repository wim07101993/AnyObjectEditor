namespace Shared.Helpers.Attributes
{
    public class TitleAttribute : AAttribute
    {
        public static readonly TitleAttribute Yes = new TitleAttribute(true);
        public static readonly TitleAttribute No = new TitleAttribute(false);
        public static readonly TitleAttribute Default = No;


        public bool Title { get; set; } = true;

        public override string Name => "title";
        public override object Value
        {
            get => Title;
            protected set => Title = (bool)value;
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