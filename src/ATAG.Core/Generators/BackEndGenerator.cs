using ATAG.Core.Generators.Writers;
using ATAG.Core.Interfaces;
using ATAG.Core.Models;
using ATAG.Core.Models.Inbound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ATAG.Core.Generators
{
    public class BackEndGenerator : BaseGenerator
    {
        private readonly BaseWriter _controllerWriter;
        private readonly BaseWriter _modelWriter;

        public BackEndGenerator(IMainParser mainParser, InputParametersModel input, BaseWriter controllerWriter, BaseWriter modelWriter)
            :base(mainParser, input)
        {
            _controllerWriter = controllerWriter;
            _modelWriter = modelWriter;
        }

        protected override void Generate(FileParseResult parseResult)
        {
            foreach (var controller in parseResult.Controllers)
            {
                if (!controller.Name.EndsWith("Controller"))
                    controller.Name += "Controller";

                string content = _controllerWriter.GenerateContent(controller);

                string fullPath = Path.Combine(_parameters.DestinationPath, $"{controller.Name}.cs");

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
