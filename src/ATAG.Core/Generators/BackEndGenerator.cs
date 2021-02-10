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
                var name = controller.Name;
                if (!name.EndsWith("Controller"))
                    name += "Controller";

                string content = _controllerWriter.GenerateContent(controller);

                string fullPath = Path.Combine(_parameters.DestinationPath, $"{name}.cs");

                BaseWriter.AddSource(fullPath, content);
            }

            foreach (var model in parseResult.Models)
            {
                string name = model.Name;
                if (!name.EndsWith("Model"))
                    name += "Model";

                string content = _modelWriter.GenerateContent(model);

                string fullPath = Path.Combine(_parameters.DestinationPath, $"{name}.cs");

                BaseWriter.AddSource(fullPath, content);
            }
        }
    }
}
