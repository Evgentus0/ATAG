using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Attributes
{
    public class FullNameAttribute: Attribute
    {
        public string FullName { get; set; }
        public FullNameAttribute(string fullName)
        {
            FullName = fullName;
        }
    }
}
