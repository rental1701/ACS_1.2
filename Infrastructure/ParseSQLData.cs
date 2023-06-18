using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Infrastructure
{
    internal static class ParseSQLData
    {
        public static DateTimeOffset? ConvertToDateTimeOffset(object value)
        {
            return value is null || value == DBNull.Value ? null : (DateTimeOffset)value;
        }

        public static DateTime? ConvertToDateTime(object? value)
        {
            return value is null || value == DBNull.Value ? null : (DateTime)value;
        }

        public static int TryParseIntBD(object? value)
        {
            if (int.TryParse(value?.ToString(), out int res))
                return res;
            else
                return 0;
        }

        public static string? TryParseStringBD(object? value)
        {
            return value is null || value == DBNull.Value ? null : (string)value;
        }

        public static double? TryParseDoubleBD(object value)
        {
            return value is null || value == DBNull.Value ? null : Math.Round((double)value, 3);
        }

        public static bool? TryParseBoolBD(object value)
        {
            return value is null || value == DBNull.Value ? null : (bool)value;
        }
    }
}
