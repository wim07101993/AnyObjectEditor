namespace Shared.Helpers.Attributes
{
    public class DisplayNameAttribute: System.ComponentModel.DisplayNameAttribute, IAttribute
    {
        public string Name => "displayName";
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
