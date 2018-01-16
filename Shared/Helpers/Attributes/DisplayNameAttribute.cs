namespace Shared.Helpers.Attributes
{
    public class DisplayNameAttribute: System.ComponentModel.DisplayNameAttribute, IAttribute
    {
        public const string NAME = "displayName";

        public string Name => NAME;
        public object Value => DisplayName;
        public object DefaultValue { get; } = null;


        public DisplayNameAttribute()
        {
        }

        public DisplayNameAttribute(string description) : base(description)
        {
        }
    }
}
