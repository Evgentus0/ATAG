using ATAG.Core.Generators.Writers;
using ATAG.Core.Interfaces;
using ATAG.Core.Models;
using ATAG.Core.Models.Inbound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Generators
{
    public class CSharpClientGenerator : BaseGenerator
    {
        private readonly BaseWriter _csClientWriter;
        private readonly BaseWriter _modelWriter;

        public CSharpClientGenerator(IMainParser mainParser, InputParametersModel input, BaseWriter csClientWriter, BaseWriter modelWriter)
            :base(mainParser, input)
        {
            _csClientWriter = csClientWriter;
            _modelWriter = modelWriter;
        }
        protected override void Generate(FileParseResult parseResult)
        {
            {
                string content = _csClientWriter.GenerateContent(parseResult.Controllers);
                string fullPath = Path.Combine(_parameters.DestinationPath, "ApiClient.cs");
                BaseWriter.AddSource(fullPath, content);
            }

            foreach (var model in parseResult.Models)
            {
                if (!model.Name.EndsWith("Model"))
                    model.Name += "Model";

                string content = _modelWriter.GenerateContent(model);

                string fullPath = Path.Combine(_parameters.DestinationPath, $"{model.Name}.cs");

                BaseWriter.AddSource(fullPath, content);
            }
        }
    }
}
