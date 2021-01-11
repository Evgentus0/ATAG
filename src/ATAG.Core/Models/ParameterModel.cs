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
        public KeyValuePair<string, string> BodyParameter { get; set; }
        public Dictionary<string, string> QueryParameters { get; set; }

        public ParameterModel()
        {
            QueryParameters = new Dictionary<string, string>();
        }

        public bool IsSemanticEquals(ParameterModel parameter)
        {
            return BodyParameter.Key == parameter.BodyParameter.Key
                && QueryParameters.Count == parameter.QueryParameters.Count
                && QueryParameters.Select(x => x.Key)
                .EqualsByElements(parameter.QueryParameters.Select(x => x.Key));
        }

        public override bool Equals(object obj)
        { 
            var parameter = (ParameterModel)obj;

            return BodyParameter.Value == parameter.BodyParameter.Value
                && QueryParameters.Count == parameter.QueryParameters.Count
                && QueryParameters.Select(x => x.Value)
                .EqualsByElements(parameter.QueryParameters.Select(x => x.Value));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
