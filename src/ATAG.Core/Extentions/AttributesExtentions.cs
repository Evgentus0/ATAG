using ATAG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Extentions
{
    public static class AttributesExtentions
    {
        public static string GetDescription(this Enum @this)
        {
            FieldInfo fi = @this.GetType().GetField(@this.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (!attributes.IsNullOrEmpty())
            {
                return attributes.First().Description;
            }

            return default;
        }

        public static Type GetTypeByKey(this Enum @this, string key)
        {
            FieldInfo fi = @this.GetType().GetField(@this.ToString());

            KeyTypeValueAttribute[] attributes = fi.GetCustomAttributes(typeof(KeyTypeValueAttribute), false) as KeyTypeValueAttribute[];

            if (!attributes.IsNullOrEmpty())
            {
                return attributes.First(x => x.Key == key)?.TypeValue ?? default;
            }

            return default;
        }
    }
}
