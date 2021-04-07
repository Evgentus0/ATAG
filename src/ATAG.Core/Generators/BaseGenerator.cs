using ATAG.Core;
using ATAG.Core.Extentions;
using ATAG.Core.Interfaces;
using ATAG.Core.Models;
using ATAG.Core.Models.Inbound;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Generators
{
    public abstract class BaseGenerator 
    {
        private const string _extention = ".atag";
        private readonly string[] _supportedTypes = { "int", "long", "string", "double", "decimal", "float" };
        private readonly IMainParser _mainParser;

        protected readonly InputParametersModel _parameters;

        public BaseGenerator(IMainParser mainParser, InputParametersModel parameters)
        {
            _mainParser = mainParser;
            _parameters = parameters;

            ValidateInputParameters();
        }

        private void ValidateInputParameters()
        {
            if(!File.Exists(_parameters.SourceFilePath))
                throw new ArgumentException($"File does not exists: {_parameters.SourceFilePath}");

            var sourceFile = _parameters.SourceFilePath;
            var extention = Path.GetExtension(sourceFile);
            if (extention != _extention)
                throw new ArgumentException($"Incorrect extention: {extention}");

            if (!Directory.Exists(_parameters.DestinationPath))
                Directory.CreateDirectory(_parameters.DestinationPath);
        }

        public void Execute()
        {
            FileParseResult parseResult = ParseFiles();

            var validationResult = ValidateModelNames(parseResult);

            if (validationResult.isSuccess)
            {
                Generate(parseResult);
            }
            else
            {
                throw new Exception(validationResult.message);
            }
        }

        protected virtual FileParseResult ParseFiles()
        {
            try
            {
                var result = _mainParser.ParseProtoFile(File.ReadAllText(_parameters.SourceFilePath));

                return result;
            }
            catch(Exception ex)
            {
                //add details
                throw new Exception("Error during parsing proto file", ex);
            }
        }

        protected virtual (bool isSuccess, string message) ValidateModelNames(FileParseResult parseResult)
        {
            var supportedTypes = new List<string>(_supportedTypes);
            var customTypes = parseResult.Models.Select(x => x.Name);
            supportedTypes.AddRange(customTypes);

            var usedTypes = new List<string>();

            foreach (var model in parseResult.Models)
            {
                var properties = model.Properties.Select(x => x.Type);

                usedTypes.AddRange(properties);
            }

            foreach(var controller in parseResult.Controllers)
            {
                foreach(var method in controller.Methods)
                {
                    if(method.ReturnedType != null)
                        usedTypes.Add(method.ReturnedType);

                    if(method.Parameters.BodyParameter.Type != null)
                        usedTypes.Add(method.Parameters.BodyParameter.Type);

                    if(!method.Parameters.QueryParameters.Select(x => x.Type).IsNullOrEmpty())
                        usedTypes.AddRange(method.Parameters.QueryParameters.Select(x => x.Type));
                }
            }

            foreach(var type in usedTypes)
            {
                if (!supportedTypes.Contains(type))
                {
                    return (false, $"Incorrect type: {type}!");
                }
            }

            return (true, string.Empty);
        }

        protected abstract void Generate(FileParseResult parseResult);

    }
}
