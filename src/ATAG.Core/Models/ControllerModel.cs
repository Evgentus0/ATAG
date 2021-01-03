using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Models
{
    public class ControllerModel
    {
        public string Name { get; set; }
        public List<MethodModel> Methods { get; set; }

        public ControllerModel()
        {
            Methods = new List<MethodModel>();
        }
    }
}
