using ATAG.Core;
using ATAG.TestInfrasctructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Generator.Test.Scenarios
{
    [TestFixture]
    public class GeneratorTests
    {
        private TestGenerator _testGenerator;

        [SetUp]
        public void Setup()
        {
            _testGenerator = new TestGenerator(new MainParser(new Core.Visitors.MainVisitor()));
        }

        [Test]
        public void GeneratorTest_ReturnGeneratedFiles()
        {
            //Arrange

            //Act
            var files = _testGenerator.GetGeneratesFiles(TestData.CorrectControllerModel.Value);

            //Assert

        }
    }
}
