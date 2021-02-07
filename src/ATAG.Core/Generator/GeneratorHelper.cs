using ATAG.Core.Extentions;
using ATAG.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Generator
{
    public static class GeneratorHelper
    {
        private const string _namespaceName = nameof(ATAG) + ".Generated";

        public static string GenerateController(ControllerModel controller)
        {
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

            sb.Append($"namespace {_namespaceName}");
            sb.AppendLine();
            sb.Append("{");

            tabLevel++;

            sb.Append($"{Tabs(tabLevel)}[Route(\"api /[controller]\")]");
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

                sb.Append($"{Tabs(tabLevel)}[{method.Verb.GetFullName()}]");
                sb.AppendLine();
                foreach (var attribute in method.Attributes)
                {
                    sb.Append($"{Tabs(tabLevel)}[{attribute.Key}(\"{attribute.Value}\")]");
                    sb.AppendLine();
                }

                sb.Append($"{Tabs(tabLevel)}public async Task<ActionResult<{method.ReturnedType}>> " +
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
            sb.Append("}");

            return sb.ToString();
        }

        public static string GenerateModel(EntityModel model)
        {
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

            sb.Append($"namespace {_namespaceName}");
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

                sb.Append($"{Tabs(tabLevel)}public {prop.Key} {prop.Value} {{get; set;}}");
                sb.AppendLine();

                tabLevel--;
            }
            sb.Append($"{Tabs(tabLevel)}}}");
            sb.AppendLine();
            sb.Append("}");

            return sb.ToString();
        }

        private static string Tabs(int n)
        {
            return new string('\t', n);
        }
    }
}