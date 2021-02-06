using ATAG.Core;
using ATAG.Core.Extentions;
using ATAG.Core.Interfaces;
using ATAG.Core.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ATAG.Generator.GeneratorHelper;

namespace ATAG.Generator
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        private const string _extention = "atag";

        private readonly string[] _supportedTypes = { "int", "long", "string", "double", "decimal", "float" };

        protected IMainParser _mainParser;

        public void Initialize(GeneratorInitializationContext context)
        {
            _mainParser = new MainParser(new Core.Visitors.MainVisitor());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            FileParseResult parseResult = ParseFiles(context);

            ValidateModelNames(parseResult);

            foreach(var controller in parseResult.Controllers)
            {
                var name = controller.Name;
                if (!name.EndsWith("Controller"))
                    name += "Controller";

                string content = GenerateController(controller);

                context.AddSource($"{name}.cs", content);
            }

            foreach (var model in parseResult.Models)
            {
                string content = GenerateModel(model);

                string name = model.Name;

                context.AddSource($"{name}.cs", content);
            }

        }

        protected virtual FileParseResult ParseFiles(GeneratorExecutionContext context)
        {
            var result = new FileParseResult();

            foreach(var file in context.AdditionalFiles
                .Where(x => Path.GetExtension(x.Path) == _extention))
            {
                var fileParse = _mainParser.ParseProtoFile(file.GetText()?.ToString());

                result.Controllers.AddRange(fileParse.Controllers);
                result.Models.AddRange(fileParse.Models);
            }

            return result;
        }

        private void ValidateModelNames(FileParseResult parseResult)
        {
            var supportedTypes = new List<string>(_supportedTypes);
            var customTypes = parseResult.Models.Select(x => x.Name);
            supportedTypes.AddRange(customTypes);

            var usedTypes = new List<string>();

            foreach (var model in parseResult.Models)
            {
                var properties = model.Properties.Select(x => x.Key);

                usedTypes.AddRange(properties);
            }

            foreach(var controller in parseResult.Controllers)
            {
                foreach(var method in controller.Methods)
                {
                    usedTypes.Add(method.ReturnedType);
                    usedTypes.Add(method.Parameters.BodyParameter.Key);
                    usedTypes.AddRange(method.Parameters.QueryParameters.Select(x => x.Key));
                }
            }

            foreach(var type in usedTypes)
            {
                if (!supportedTypes.Contains(type))
                {
                    throw new ArgumentException($"Incorrect type: {type}!");
                }
            }
        }
    }
}
