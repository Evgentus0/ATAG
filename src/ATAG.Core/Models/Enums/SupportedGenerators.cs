using ATAG.Core.Attributes;
using ATAG.Core.Generators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATAG.Core.Models.Enums
{
    public enum SupportedGenerators
    {
        [KeyTypeValue("Generator", typeof(BackEndGenerator))]
        BackEnd,
        CSharpClient,
        AngularClient
    }
}
