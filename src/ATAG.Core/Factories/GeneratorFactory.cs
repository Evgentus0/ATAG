using ATAG.Core.Generators;
using ATAG.Core.Generators.Writers;
using ATAG.Core.Models.Enums;
using ATAG.Core.Models.Inbound;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATAG.Core.Factories
{
    public class GeneratorFactory
    {
        public BaseGenerator GetGeneratorInstance(InputParametersModel paramters)
        {
            switch (paramters.GeneratorType)
            {
                case SupportedGenerators.BackEnd:
                    return new BackEndGenerator(new MainParser(new Visitors.MainVisitor()), 
                        paramters, new ControllerWriter(paramters.Namespace), new ModelWriter(paramters.Namespace));
                case SupportedGenerators.CSharpClient:
                    return new CSharpClientGenerator(new MainParser(new Visitors.MainVisitor()),
                        paramters, new CsClientWriter(paramters.Namespace), new ModelWriter(paramters.Namespace));
                case SupportedGenerators.AngularClient:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException($"Incorrect geenrator type {paramters.GeneratorType}");
            }
        }
    }
}
