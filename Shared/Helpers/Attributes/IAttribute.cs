namespace Shared.Helpers.Attributes
{
    public interface IAttribute
    {
        string Name { get; }
        object Value { get; }
        object DefaultValue { get; }
    }
}