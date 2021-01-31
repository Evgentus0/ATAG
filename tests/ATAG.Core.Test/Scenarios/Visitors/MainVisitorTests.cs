using Antlr4.Runtime;
using ATAG.Core.Visitors;
using ATAG.TestInfrasctructure;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Test.Scenarios.Visitors
{
    [TestFixture]
    public class MainVisitorTests
    {
        private MainParser _mainParser;

        [SetUp]
        public void Setup()
        {
            _mainParser = new MainParser(new MainVisitor());
        }

        [Test]
        public void test()
        {
            Dictionary<string, int> test1 = new Dictionary<string, int>
            {
                ["str1"] = 2,
                ["str2"] = 2
            };
            Dictionary<string, int> test2 = new Dictionary<string, int>
            {
                ["str1"] = 1,
                ["str2"] = 2
            };


            var t = test1.First().Equals(test2.First());

            var t2 = test2.Contains(test1.First());
        }

        [Test]
        public void VisitControllerTest()
        {
            //Arrange

            var expression =
@"
cntrl ControllerName1{
	[Route=test_fdf_tedsf] get MethodName1() return ResponseModelName1;
	post MethodName2(fromBody: string StrParameter; fromQuery: int qpInt, string qpStr) return ResponseModelName2;
}
";
            var generator = new TestGenerator(_mainParser, expression);
            //Act


            generator.Execute(new GeneratorExecutionContext());

            //Assert
            
        }
    }
}
