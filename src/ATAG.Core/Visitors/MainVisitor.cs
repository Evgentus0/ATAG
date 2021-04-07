using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ATAG.Core.Exceptions;
using ATAG.Core.Models;
using ATAG.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Visitors
{
    public class MainVisitor : GrammarBaseVisitor<object>
    {
        private readonly string[] _primitives = { "string", "int", "double", "long", "decimal" };

        private readonly string[] _supportedAttributes = { "Route" };

        public override object VisitInstructions([NotNull] GrammarParser.InstructionsContext context)
        {
            var result = new FileParseResult();

            if (context == null)
                return result;

            if (context.exception is not null)
                throw context.exception;

            try
            {
                foreach (var child in context.children)
                {
                    var instructionType = child.GetChild(0).GetText();

                    switch (instructionType)
                    {
                        case "cntrl":
                            var controller = (ControllerModel)Visit(child);

                            ShouldBeUnique("Controller", controller.Name, 
                                result.Controllers.Select(x => x.Name), child);

                            result.Controllers.Add(controller);
                            break;

                        case "model":
                            var model = (EntityModel)Visit(child);

                            ShouldBeUnique("Model", model.Name,
                                result.Models.Select(x => x.Name), child);

                            result.Models.Add(model);
                            break;

                        default:
                            throw new GrammarException($"Can not parse isntruction {instructionType}") 
                            {
                                FromLine = child.SourceInterval.a,
                                ToLine = child.SourceInterval.b
                            };
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public override object VisitController([NotNull] GrammarParser.ControllerContext context)
        {
            var controller = new ControllerModel();

            if (context == null)
                return controller;

            if (context.exception is not null)
                throw context.exception;

            controller.Name = context.name.Text;

            try
            {
                foreach (var method in context.methodDefinition())
                {
                    var methodModel = (MethodModel)Visit(method);

                    ShouldBeUnique("Method", methodModel, controller.Methods, method);

                    controller.Methods.Add(methodModel);
                }

                return controller;
            }
            catch
            {
                throw;
            }
        }

        public override object VisitMethodDefinition([NotNull] GrammarParser.MethodDefinitionContext context)
        {
            var methodModel = new MethodModel();

            if (context == null)
                return methodModel;

            if (context.exception is not null)
                throw context.exception;

            methodModel.Verb = (HttpVerb)Enum.Parse(typeof(HttpVerb), context.verb.Text);
            methodModel.Name = context.name.Text;
            methodModel.ReturnedType = context.returnedType.Text;
            methodModel.Route = context.route?.Text ?? string.Empty;

            if (!string.IsNullOrEmpty(methodModel.Route))
            {
                methodModel.Route = methodModel.Route.Trim(new char[]{' ', '"'});
            }

            methodModel.Parameters = context.parameters() != null ?
                (ParameterModel)Visit(context.parameters()) : new ParameterModel();


            return methodModel;
        }

        public override object VisitModel([NotNull] GrammarParser.ModelContext context)
        {
            var model = new EntityModel();

            if (context == null)
                return model;

            if (context.exception is not null)
                throw context.exception;

            model.Name = context.name.Text;

            foreach(var prop in context.propertyDefenition())
            {
                var property = (FieldModel)Visit(prop);

                ShouldBeUnique("Property", property,
                    model.Properties, prop);

                model.Properties.Add(new FieldModel { Type = property.Type, Name = property.Name });
            }


            return model;
        }

        public override object VisitPropertyDefenition([NotNull] GrammarParser.PropertyDefenitionContext context)
        {
            if (context == null)
                return new FieldModel();

            if (context.exception is not null)
                throw context.exception;

            return new FieldModel { Type = context.type.Text, Name = context.name.Text };
        }

        public override object VisitParameters([NotNull] GrammarParser.ParametersContext context)
        {
            var parameters = new ParameterModel();
            if (context == null)
                return parameters;

            if (context.exception is not null)
                throw context.exception;

            parameters.BodyParameter = context.bodyParameter() != null ?
                (FieldModel)Visit(context.bodyParameter()) : new FieldModel();
            parameters.QueryParameters = context.queryParameter() != null ?
                (List<FieldModel>)Visit(context.queryParameter()) : new List<FieldModel>();

            return parameters;
        }

        public override object VisitQueryParameter([NotNull] GrammarParser.QueryParameterContext context)
        {
            var parameters = new List<FieldModel>();
            if (context == null)
                return parameters;

            if (context.exception is not null)
                throw context.exception;

            foreach (var prop in context.propertyDefenition())
            {
                var property = (FieldModel)Visit(prop);

                ShouldBeUnique("QueryParameter", property.Name, 
                    parameters.Select(x => x.Name), prop);

                parameters.Add(new FieldModel{ Type = property.Type, Name = property.Name});
            }

            return parameters;
        }

        public override object VisitBodyParameter([NotNull] GrammarParser.BodyParameterContext context)
        {
            if (context == null)
                return new FieldModel();

            if (context.exception is not null)
                throw context.exception;

            var keyValue = (FieldModel)Visit(context.propertyDefenition());
            return keyValue;
        }

        private void ShouldBeUnique<T>(string memberName, T suspect, 
            IEnumerable<T> collection, IParseTree currentTree)
        {
            if(collection.Contains(suspect))
                throw new GrammarException($"{memberName} with name \"{suspect}\" is already exist!")
                {
                    FromLine = currentTree.SourceInterval.a,
                    ToLine = currentTree.SourceInterval.b
                };
        }
    }
}
