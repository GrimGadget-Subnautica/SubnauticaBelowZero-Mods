using System;

namespace Grimware
{
    internal static class StringExtensions
    {
        public static TEnum? ToEnum<TEnum>(this string value)
            where TEnum : struct, Enum
        {
            return ToEnum<TEnum>(value, false);
        }

        public static TEnum? ToEnum<TEnum>(this string value, bool ignoreCase)
            where TEnum : struct, Enum
        {
            return Enum.TryParse(value, ignoreCase, out TEnum result) ? result : (TEnum?)null;
        }
    }
}
