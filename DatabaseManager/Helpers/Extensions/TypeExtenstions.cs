using System;

namespace DatabaseManager.Helpers.Extensions
{
    public static class TypeExtenstions
    {
        public static bool IsNativeType(this Type This)
            => This == typeof(string) || This == typeof(char) ||
               This == typeof(bool) ||
               This == typeof(sbyte) || This == typeof(byte) ||
               This == typeof(short) || This == typeof(ushort) ||
               This == typeof(int) || This == typeof(uint) ||
               This == typeof(long) || This == typeof(ulong) ||
               This == typeof(decimal) ||
               This == typeof(double) ||
               This == typeof(float);
    }
}
