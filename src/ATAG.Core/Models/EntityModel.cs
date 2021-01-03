using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Models
{
    public class EntityModel
    {
        public string Name { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public EntityModel()
        {
            Properties = new Dictionary<string, string>();
        }
    }
}
