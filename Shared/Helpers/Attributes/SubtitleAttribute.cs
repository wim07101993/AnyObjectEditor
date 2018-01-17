namespace Shared.Helpers.Attributes
{
    public class SubtitleAttribute : AAttribute
    {
        public static readonly SubtitleAttribute Yes = new SubtitleAttribute(true);
        public static readonly SubtitleAttribute No = new SubtitleAttribute(false);
        public static readonly SubtitleAttribute Default = No;
        public const string NAME = "subtitle";


        public bool IsSubtitle { get; private set; }

        public override string Name => NAME;
        public override object Value
        {
            get => IsSubtitle;
            protected set => IsSubtitle = (bool)value;
        }

        public override object DefaultValue { get; } = true;


        public SubtitleAttribute()
        {
        }

        public SubtitleAttribute(bool subtitle) : base(subtitle)
        {
        }
    }
}