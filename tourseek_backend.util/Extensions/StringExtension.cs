using System;
using System.Globalization;
using System.Text.RegularExpressions;
using tourseek_backend.services.Validators;
using tourseek_backend.util.Strings;

namespace tourseek_backend.util.Extensions
{
    public static class StringExtension
    {
        public static bool ParseRawDate(this string dateString, out DateTime? date)
        {
            date = null;
            try
            {
                var status = double.TryParse(dateString, out var d);
                if (status) date = DateTime.FromOADate(d);

                return status;
            }
            catch
            {
                return false;
            }
        }
        public static bool ParseDate(this string dateString, out DateTime? date)
        {
            return ParseDate(dateString, "dd/MM/yyyy", out date);
        }

        public static bool ParseDate(this string dateString, string format, out DateTime? date)
        {
            var status = DateTime.TryParseExact(dateString, format, System.Globalization.DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None, out var dateValue);
            date = !status ? (DateTime?) null : dateValue;
            try
            {
                if (date == null)
                {
                    date =  Convert.ToDateTime(dateString);
                }
            }
            catch(Exception)
            {
                date = null;
            }
            return status;
        }

        public static bool ParseInt(this string s, out int? i)
        {
            var status = int.TryParse(s, out var iValue);
            i = status ? (int?) null : iValue;

            return status;
        }

        public static bool ParseDouble(this string s, out double? i)
        {
            var status = double.TryParse(s, out var iValue);
            i = status ? (double?) null : iValue;

            return status;
        }

        public static bool ParseDecimal(this string s, out decimal? i)
        {
            var status = decimal.TryParse(s, out var iValue);
            i = status ? (decimal?) null : iValue;

            return status;
        }

        public static string ToLowerFirstChar(this string input)
        {
            string newString = input;
            if (!String.IsNullOrEmpty(newString) && Char.IsUpper(newString[0]))
                newString = Char.ToLower(newString[0]) + newString.Substring(1);
            return newString;
        }

        public static bool ValidateType(this string value, StringType type)
        {
            if (type != null && !string.IsNullOrWhiteSpace(value) && type != StringType.String)
            {
                if (type == StringType.Integer)
                {
                    return ParseInt(value, out _);
                }

                if (type == StringType.Decimal)
                {
                    return ParseDecimal(value, out _);
                }

                if (type == StringType.DateTime)
                {
                    
                    return ParseRawDate(value, out _);
                }
            }

            return true;
        }

        public static bool ValidateFormat(this string value, StringFormat format)
        {
            if (format != null && !string.IsNullOrWhiteSpace(value))
            {
                return Regex.IsMatch(value, $"(?i)^{format.RegexFormat}$");
            }

            return true;
        }
    }
}