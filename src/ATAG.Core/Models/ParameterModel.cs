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
    }
}
