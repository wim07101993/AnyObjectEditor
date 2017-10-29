using System.Linq;

namespace DatabaseManager.Helpers
{
    public static class ObjectExtensions
    {
        public static bool HasNativeType(this object This)
            => TypeExtensions.NativeTypes.Any(x => x == This.GetType());
    }
}