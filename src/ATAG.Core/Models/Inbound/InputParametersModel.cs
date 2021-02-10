using ATAG.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATAG.Core.Models.Inbound
{
    public class InputParametersModel
    {
        public string SourceFilePath { get; set; }
        public string DestinationPath { get; set; }
        public SupportedGenerators GeneratorType { get; set; }
    }
}
