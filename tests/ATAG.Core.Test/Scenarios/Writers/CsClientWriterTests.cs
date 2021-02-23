using ATAG.Core.Generators.Writers;
using ATAG.TestInfrasctructure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Test.Scenarios.Writers
{
    [TestFixture]
    public class CsClientWriterTests
    {
        private CsClientWriter _writer;

        [SetUp]
        public void Setup()
        {
            _writer = new CsClientWriter("TestNamespace");
        }

        [Test]
        public void Writer_Test()
        {
            var controllers = TestData.CorrectControllerModel.Value.Controllers;

            var content = _writer.GenerateContent(controllers);
        }
    }
}
