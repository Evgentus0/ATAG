using ATAG.Core.Extentions;
using ATAG.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATAG.Core.Generators.Writers
{
    public class ControllerWriter : BaseWriter
    {
        public ControllerWriter(string @namespace): base(@namespace)
        { }

        public override string GenerateContent(object entity)
        {
            var controller = (ControllerModel)entity;

            StringBuilder sb = new StringBuilder();
            int tabLevel = 0;

            sb.Append("using System;");
            sb.AppendLine();
            sb.Append("using System.Collections.Generic;");
            sb.AppendLine();
            sb.Append("using System.Linq;");
            sb.AppendLine();
            sb.Append("using System.Threading.Tasks;");
            sb.AppendLine();
            sb.Append("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine();
            sb.Append("using Microsoft.AspNetCore.Http;");
            sb.AppendLine();
            sb.Append("using System.Runtime;");
            sb.AppendLine();

            sb.Append($"namespace {_namespace}");
            sb.AppendLine();
            sb.Append("{");
            sb.AppendLine();

            tabLevel++;

            sb.Append($"{Tabs(tabLevel)}[Route(\"api/[controller]\")]");
            sb.AppendLine();
            sb.Append($"{Tabs(tabLevel)}[ApiController]");
            sb.AppendLine();
            sb.Append($"{Tabs(tabLevel)}public abstract class {controller.Name} : ControllerBase");
            sb.AppendLine();
            sb.Append($"{Tabs(tabLevel)}{{");
            sb.AppendLine();

            foreach (var method in controller.Methods)
            {
                tabLevel++;

                sb.Append($"{Tabs(tabLevel)}[{method.Verb.GetDescription()}]");
                sb.AppendLine();
                if (!string.IsNullOrEmpty(method.Route))
                {
                    sb.Append($"{Tabs(tabLevel)}[Route(\"{method.Route}\")]");
                    sb.AppendLine();
                }

                sb.Append($"{Tabs(tabLevel)}public abstract Task<ActionResult<{method.ReturnedType}>> " +
                    $"{method.Name}(");

                bool hasBodyParameter = !string.IsNullOrEmpty(method.Parameters.BodyParameter.Key);
                if (hasBodyParameter)
                {
                    var bp = method.Parameters.BodyParameter;
                    sb.Append($"[FromBody]{bp.Key} {bp.Value}");
                }

                if (method.Parameters.QueryParameters.Count > 0)
                {
                    if (hasBodyParameter)
                        sb.Append(", ");

                    foreach (var qp in method.Parameters.QueryParameters)
                    {
                        sb.Append($"[FromQuery]{qp.Key} {qp.Value}, ");
                    }
                    sb.Length -= 2;
                }
                sb.Append(");");

                sb.AppendLine();
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
