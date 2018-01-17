using System;

namespace Shared.Helpers.Extensions
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
        
        public static object GetDefaultValue(this ENativeType This)
        {
            switch (This)
            {
                case ENativeType.Bool:
                    return default(bool);
                case ENativeType.String:
                    return null;
                case ENativeType.Char:
                    return default(char);
                case ENativeType.Byte:
                    return default(byte);
                case ENativeType.SByte:
                    return default(sbyte);
                case ENativeType.Short:
                    return default(short);
                case ENativeType.UShort:
                    return default(ushort);
                case ENativeType.Int:
                    return default(int);
                case ENativeType.UInt:
                    return default(uint);
                case ENativeType.Long:
                    return default(ulong);
                case ENativeType.ULong:
                    return default(decimal);
                case ENativeType.Float:
                    return default(float);
                case ENativeType.Decimal:
                    break;
                case ENativeType.Double:
                    return default(double);
                default:
                    throw new ArgumentOutOfRangeException(nameof(This), This, null);
            }
            return null;
        }
    }
}