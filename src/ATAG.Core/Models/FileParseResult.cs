using ATAG.Core.Extentions;
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

        public override bool Equals(object obj)
        {
            var result = (FileParseResult)obj;

            return Controllers.HasSameElements(result.Controllers)
                && Models.HasSameElements(result.Models);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
