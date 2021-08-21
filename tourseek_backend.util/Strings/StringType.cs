using System;

namespace tourseek_backend.util.Strings
{
    public class StringType
    {
        public static readonly StringType Decimal = new StringType(typeof(decimal));
        public static readonly StringType Integer = new StringType(typeof(int));
        public static readonly StringType String = new StringType(typeof(string));
        public static readonly StringType DateTime = new StringType(typeof(DateTime));

        public Type Type { get; }

        public StringType(Type type)
        {
            Type = type;
        }
    }
}