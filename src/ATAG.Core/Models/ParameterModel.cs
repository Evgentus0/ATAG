using ATAG.Core.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Models
{
    public class ParameterModel
    {
        public FieldModel BodyParameter { get; set; }
        public List<FieldModel> QueryParameters { get; set; }

        public ParameterModel()
        {
            BodyParameter = new FieldModel();
            QueryParameters = new List<FieldModel>();
        }

        public override bool Equals(object obj)
        { 
            var parameter = (ParameterModel)obj;

            return BodyParameter.Type == parameter.BodyParameter.Type
                    && QueryParameters.Count == parameter.QueryParameters.Count
                    && QueryParameters.Select(x => x.Type)
                    .EqualsByElements(parameter.QueryParameters.Select(x => x.Type));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
