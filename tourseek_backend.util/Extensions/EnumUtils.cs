using System;
using System.ComponentModel;
using System.Linq;

namespace tourseek_backend.util.Extensions
{
    public static class EnumUtils
    {
        public static string GetEnumDescription(this Enum e)
        {
            // get the type of that enum.
            Type type = e.GetType();

            // get all the value in that enum.
            Array enumValues = Enum.GetValues(type);
            var info = type.GetMember(e.ToString());
            var description = info[0]
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;
            return description.Description;
            //foreach (int value in enumValues)
            //{

            //    var info = type.GetMember(type.GetEnumName(value));
            //    var description = info[value]
            //        .GetCustomAttributes(typeof(DescriptionAttribute), false)
            //        .FirstOrDefault() as DescriptionAttribute;
            //    if (description != null)
            //    {
            //        return description.Description;
            //    }
            //}

            //return null;

        }
    }
}
