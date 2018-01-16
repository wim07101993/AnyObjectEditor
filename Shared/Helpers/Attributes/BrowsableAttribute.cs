namespace Shared.Helpers.Attributes
{
    public class BrowsableAttribute : AAttribute
    {
        public static readonly BrowsableAttribute Yes = new BrowsableAttribute(true);
        public static readonly BrowsableAttribute No = new BrowsableAttribute(false);
        public static readonly BrowsableAttribute Default = Yes;
        public const string NAME = "browsable";


        public bool Browsable { get; private set; }

        public override string Name => NAME;

        public override object Value
        {
            get => Browsable;
            protected set => Browsable = (bool) value;
        }

        public override object DefaultValue { get; } = true;


        public BrowsableAttribute(bool browsable) : base(browsable)
        {
        }
    }
}