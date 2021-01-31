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
        private string _testText;

        public TestGenerator(IMainParser mainParser, string testText): base(mainParser)
        {
            _testText = testText;
        }

        protected override FileParseResult ParseFiles(GeneratorExecutionContext context)
        {

            var result = _mainParser.ParseProtoFile(_testText);

            return result;
        }

    }
}
