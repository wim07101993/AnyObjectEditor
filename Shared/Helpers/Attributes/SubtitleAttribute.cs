namespace Shared.Helpers.Attributes
{
    public class SubtitleAttribute : AAttribute
    {
        public static readonly SubtitleAttribute Yes = new SubtitleAttribute(true);
        public static readonly SubtitleAttribute No = new SubtitleAttribute(false);
        public static readonly SubtitleAttribute Default = No;


        public bool Subtitle { get; private set; }

        public override string Name => "subtitle";
        public override object Value
        {
            get => Subtitle;
            protected set => Subtitle = (bool)value;
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