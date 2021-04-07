using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Models
{
    public class FieldModel
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var field = (FieldModel)obj;

            return Type == field.Type
                && Name == field.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
