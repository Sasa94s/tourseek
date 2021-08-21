using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace tourseek_backend.services.Validators
{
    public class StringFormat
    {
        private const string EmailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        private const string EmailExample = "jsmith@example.com";
        private const string HoursRegex = @"\d+ h";
        private const string HoursExample = "## h";
        
        public string RegexFormat { get; }
        public string ReadableFormat { get; }

        private StringFormat(string regexFormat, string readableFormat)
        {
            RegexFormat = regexFormat;
            ReadableFormat = readableFormat;
        }

        private static string Xsv(string x, params string[] values)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                sb.Append(values[i]);
                if (i != values.Length - 1)
                {
                    sb.Append(x);
                }
            }

            return sb.ToString();
        }

        public static StringFormat Email()
        {
            return new StringFormat(EmailRegex, EmailExample);
        }

        public static StringFormat Hours()
        {
            return new StringFormat(HoursRegex, HoursExample);
        }

        public static StringFormat Choice(params string[] choices)
        {
            return new StringFormat($"({Xsv("|", choices.Select(Regex.Escape).ToArray())})", Xsv("/", choices));
        }

        public static StringFormat CustomRegex(string regex, string format)
        {
            return new StringFormat(regex, format);
        }
    }
}