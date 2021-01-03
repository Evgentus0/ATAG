using Antlr4.Runtime;
using ATAG.Core.Visitors;
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
        public void VisitControllerTest()
        {
            //Arrange
            var expression =
@"
cntrl ControllerName1{
	[route=test_fdf_tedsf] get MethodName1() return ResponseModelName1;
	post MethodName2(fromBody: string StrParameter; fromQuery: int qpInt, string qpStr) return ResponseModelName2;
}
";
            //Act
            var result = _mainParser.ParseProtoFile(expression);

            //Assert
            Assert.AreEqual(0, result.Models.Count);
            Assert.AreEqual(1, result.Controllers.Count);

            var controller = result.Controllers.First();
            Assert.AreEqual("ControllerName1", controller.Name);
        }
    }
}
