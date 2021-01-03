using ATAG.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Interfaces
{
    public interface IMainParser
    {
        FileParseResult ParseProtoFile(string content);
    }
}
