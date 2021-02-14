using ATAG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Models.Enums
{
    public enum HttpVerb
    {
        [Description("HttpGet")]
        get,
        [Description("HttpPost")]
        post,
        [Description("HttpPut")]
        put,
        [Description("HttpDelete")]
        delete
    }
}
