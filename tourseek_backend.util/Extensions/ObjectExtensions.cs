using System;
using System.Linq;

namespace tourseek_backend.util.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsAllNullOrEmpty(this object obj)
        {
            if (ReferenceEquals(obj, null))
                return true;

            return obj.GetType().GetProperties()
                .All(x => IsNullOrEmpty(x.GetValue(obj)));
        }
        
        // Reference: https://codereview.stackexchange.com/questions/147856/generic-null-empty-check-for-each-property-of-a-class
        public static bool IsAnyNullOrEmpty(this object obj)
        {
            if (ReferenceEquals(obj, null))
                return true;

            return obj.GetType().GetProperties()
                .Any(x => IsNullOrEmpty(x.GetValue(obj)));
        }
        private static bool IsNullOrEmpty(this object value)
        {
            if (ReferenceEquals(value, null))
                return true;

            var type = value.GetType();
            return type.IsValueType
                   && Equals(value, Activator.CreateInstance(type));
        }
    }
}