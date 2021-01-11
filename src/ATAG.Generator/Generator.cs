using ATAG.Core.Extentions;
using ATAG.Core.Interfaces;
using ATAG.Core.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Generator
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        private const string _extention = "atag";

        private IMainParser _mainParser;

        public Generator(IMainParser mainParser)
        {
            _mainParser = mainParser;
        }

        public void Initialize(GeneratorInitializationContext context)
        {

        }

        public void Execute(GeneratorExecutionContext context)
        {
            FileParseResult parseResult = ParseFiles(context);

            ValidateModelNames(parseResult);

            foreach(var controller in parseResult.Controllers)
            {
                string content = GenerateController(controller);
                var name = controller.Name;
                if (!name.EndsWith("Controller"))
                    name += "Controller";

                context.AddSource($"{name}.cs", content);
            }

            foreach (var model in parseResult.Models)
            {
                string content = GenerateModel(model);

                string name = model.Name;

                context.AddSource($"{name}.cs", content);
            }

        }

        private FileParseResult ParseFiles(GeneratorExecutionContext context)
        {
            var result = new FileParseResult();

            foreach(var file in context.AdditionalFiles
                .Where(x => Path.GetExtension(x.Path) == _extention))
            {
                var fileParse = _mainParser.ParseProtoFile(file.GetText()!.ToString());

                result.Controllers.AddRange(fileParse.Controllers);
                result.Models.AddRange(fileParse.Models);
            }

            return result;
        }

        private void ValidateModelNames(FileParseResult parseResult)
        {
            throw new NotImplementedException();
        }

        private string GenerateController(ControllerModel controller)
        {
            StringBuilder sb = new StringBuilder();
            int tabLevel = 0;

            sb.Append("[Route(\"api /[controller]\")]");
            sb.AppendLine();
            sb.Append("[ApiController]");
            sb.AppendLine();
            sb.Append($"public abstract class {controller.Name} : ControllerBase");
            sb.AppendLine();
            sb.Append("{");
            sb.AppendLine();

            foreach (var method in controller.Methods)
            {
                tabLevel++;

                sb.Append($"[{Tabs(tabLevel)}{method.Verb.GetFullName()}]");
                sb.AppendLine();
                foreach (var attribute in method.Attributes)
                {
                    sb.Append($"[{Tabs(tabLevel)}{attribute.Key}(\"{attribute.Value}\")]");
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

                if(method.Parameters.QueryParameters.Count > 0)
                {
                    if (hasBodyParameter)
                        sb.Append(", ");

                    foreach(var qp in method.Parameters.QueryParameters)
                    {
                        sb.Append($"{qp.Key} {qp.Value}, ");
                    }
                    sb.Length -= 2;
                }
                sb.Append(");");

                sb.AppendLine();
                sb.AppendLine();
            }

            sb.Append("}");

            return sb.ToString();
        }

        private string GenerateModel(EntityModel model)
        {
            var sb = new StringBuilder();

            sb.Append($"public class {model.Name}");
            sb.AppendLine();
            sb.Append("{");

            int tabLevel = 0;
            foreach (var prop in model.Properties)
            {
                tabLevel++;

                sb.Append($"{Tabs(tabLevel)}public {prop.Key} {prop.Value} {{get; set;}}");
                sb.AppendLine();
            }
            sb.Append("}");

            return sb.ToString();
        }

        static string Tabs(int n)
        {
            return new string('\t', n);
        }
    }
}
