using ATAG.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATAG.Core.Generators.Writers
{
    class ModelWriter : BaseWriter
    {
        public ModelWriter(string @namespace) : base(@namespace)
        { }
        public override string GenerateContent(object entity)
        {
            var model = (EntityModel)entity;

            var sb = new StringBuilder();
            int tabLevel = 0;

            sb.Append("using System;");
            sb.AppendLine();
            sb.Append("using System.Collections.Generic;");
            sb.AppendLine();
            sb.Append("using System.Linq;");
            sb.AppendLine();
            sb.Append("using System.Runtime;");
            sb.AppendLine();

            sb.Append($"namespace {_namespace}");
            sb.AppendLine();
            sb.Append("{");
            sb.AppendLine();

            tabLevel++;

            sb.Append($"{Tabs(tabLevel)}public class {model.Name}");
            sb.AppendLine();
            sb.Append($"{Tabs(tabLevel)}{{");
            sb.AppendLine();

            foreach (var prop in model.Properties)
            {
                tabLevel++;

                sb.Append($"{Tabs(tabLevel)}public {prop.Type} {prop.Name} {{get; set;}}");
                sb.AppendLine();

                tabLevel--;
            }
            sb.Append($"{Tabs(tabLevel)}}}");
            sb.AppendLine();
            sb.Append("}");

            return sb.ToString();
        }
    }
}
