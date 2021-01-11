using ATAG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Extentions
{
    public static class AttributesExtentions
    {
        public static string GetFullName(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            FullNameAttribute[] attributes = fi.GetCustomAttributes(typeof(FullNameAttribute), false) as FullNameAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().FullName;
            }

            return value.ToString();
        }
    }
}
