using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATAG.Core.Interfaces;
using ATAG.Core.Models;
using Microsoft.CodeAnalysis;

namespace ATAG.TestInfrasctructure
{
    public class TestGenerator: Generator.Generator
    {
        public TestGenerator(IMainParser mainParser): base(mainParser)
        {   }

        public Dictionary<string, string> GetGeneratesFiles(FileParseResult model)
        {
            var result = new Dictionary<string, string>();

            foreach(var cntrl in model.Controllers)
            {
                result.Add(cntrl.Name, GenerateController(cntrl));
            }

            foreach(var entity in model.Models)
            {
                result.Add(entity.Name, GenerateModel(entity));
            }

            return result;
        }
    }
}
