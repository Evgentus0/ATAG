using ATAG.Core.Extentions;
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

        public override bool Equals(object obj)
        {
            var model = (EntityModel)obj;

            return Name == model.Name
                && Properties
                .HasSameElements(model.Properties);
        }
    }
}
