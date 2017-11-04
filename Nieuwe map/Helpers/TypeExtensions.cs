using System;
using System.Linq;

namespace DatabaseManager.Helpers
{
    public static class TypeExtensions
    {
        public static Type[] NativeTypes =
        {
            typeof(bool),
            typeof(string), typeof(char),
            typeof(byte), typeof(sbyte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),
            typeof(decimal),
            typeof(double),
            typeof(float),
        };

        public static bool IsNativType(this Type This)
            => NativeTypes.Any(x => x == This);
    }
}