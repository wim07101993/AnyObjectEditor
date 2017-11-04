namespace DatabaseManager.Extensions
{
    public static class ObjectExtensions
    {
        public static bool HasNativeType(this object This)
            => This.GetType().IsNativType();
    }
}
