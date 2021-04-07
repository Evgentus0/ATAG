using System;
using System.Collections.Generic;
using System.Text;

namespace ATAG.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class KeyTypeValueAttribute:Attribute
    {
        public string Key { get; set; }
        public Type TypeValue { get; set; }

        public KeyTypeValueAttribute(string key, Type type)
        {
            Key = key;
            TypeValue = type;
        }
    }
}
