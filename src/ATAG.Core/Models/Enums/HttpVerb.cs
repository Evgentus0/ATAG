using ATAG.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Models.Enums
{
    public enum HttpVerb
    {
        [FullName("HttpGet")]
        get,
        [FullName("HttpPost")]
        post,
        [FullName("HttpPut")]
        put,
        [FullName("HttpDelete")]
        delete
    }
}
