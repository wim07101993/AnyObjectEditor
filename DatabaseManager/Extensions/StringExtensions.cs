using System;
using DatabaseManager.Properties;

namespace DatabaseManager.Extensions
{
    public static class StringExtensions
    {
        public static ENativeType ConvertToENativeType(this string This)
        {
            foreach (var t in Enum.GetValues(typeof(ENativeType)))
                if (Equals(This, t))
                    return (ENativeType) t;

            switch (This.ToLower())
            {
                case "boolean": return ENativeType.Bool;
                case "string": return ENativeType.String;
                case "char": return ENativeType.Char;
                case "byte": return ENativeType.Byte;
                case "sbyte": return ENativeType.SByte;
                case "int16": return ENativeType.Short;
                case "uint16": return ENativeType.UShort;
                case "int32": return ENativeType.Int;
                case "uint32": return ENativeType.UInt;
                case "int64": return ENativeType.Long;
                case "uint64": return ENativeType.ULong;
                case "single": return ENativeType.Float;
                case "decimal": return ENativeType.Decimal;
                case "double": return ENativeType.Double;
                default:
                    throw new ArgumentException(string.Format(Resources.ValueNotFouneAsElementOfEnum, This), This);
            }
        }
    }
}