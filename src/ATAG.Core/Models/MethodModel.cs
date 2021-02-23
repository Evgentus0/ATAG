using ATAG.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Models
{
    public class MethodModel
    {
        public HttpVerb Verb { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public string ReturnedType { get; set; }
        public ParameterModel Parameters { get; set; }

        public override bool Equals(object obj)
        {
            var method = (MethodModel)obj;

            return Name == method.Name
                && Parameters.Equals(method.Parameters);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
