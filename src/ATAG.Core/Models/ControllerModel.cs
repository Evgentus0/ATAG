using ATAG.Core.Extentions;
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

        public override bool Equals(object obj)
        {
            var controller = (ControllerModel)obj;

            return Name == controller.Name
                && Methods.HasSameElements(controller.Methods);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
