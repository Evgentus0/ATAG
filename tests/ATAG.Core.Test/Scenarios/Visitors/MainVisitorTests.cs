using Antlr4.Runtime;
using ATAG.Core.Models;
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
        public void VisitController_returnCorrectModel_Test()
        {
            //Arrange
            var model = TestData.CorrectControllerModel;

            var expression = model.Key;
            var excpectedModel = model.Value;

            //Act
            var result = _mainParser.ParseProtoFile(expression);

            //Assert
            Assert.AreEqual(excpectedModel, result);
        }
    }
}
