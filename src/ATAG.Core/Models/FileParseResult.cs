using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Models
{
    public class FileParseResult
    {
        public List<ControllerModel> Controllers { get; set; }
        public List<EntityModel> Models { get; set; }

        public FileParseResult()
        {
            Controllers = new List<ControllerModel>();
            Models = new List<EntityModel>();
        }
    }
}
