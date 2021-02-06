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

            methodModel.Verb = (HttpVerb)Enum.Parse(typeof(HttpVerb), context.VERB().GetText());
            methodModel.Name = context.name.Text;
            methodModel.ReturnedType = context.returnedType.Text;
            methodModel.Attributes = context.attributes() != null ?
                (Dictionary<string, string>)Visit(context.attributes()) : new Dictionary<string, string>();
            methodModel.Parameters = context.parameters() != null ?
                (ParameterModel)Visit(context.parameters()) : new ParameterModel();


            return methodModel;
        }

        public override object VisitModel([NotNull] GrammarParser.ModelContext context)
        {
            var model = new EntityModel();

            if (context == null)
                return model;

            model.Name = context.name.Text;

            foreach(var prop in context.propertyDefenition())
            {
                var property = (KeyValuePair<string, string>)Visit(prop);

                ShouldBeUnique("Property", property,
                    model.Properties, prop);

                model.Properties.Add(property.Key, property.Value);
            }


            return model;
        }

        public override object VisitPropertyDefenition([NotNull] GrammarParser.PropertyDefenitionContext context)
        {
            if (context == null)
                return new KeyValuePair<string, string>();

            return new KeyValuePair<string, string>(context.type.Text, context.name.Text);
        }

        public override object VisitParameters([NotNull] GrammarParser.ParametersContext context)
        {
            var parameters = new ParameterModel();
            if (context == null)
                return parameters;

            parameters.BodyParameter = context.bodyParameter() != null ?
                (KeyValuePair<string, string>)Visit(context.bodyParameter()) : new KeyValuePair<string, string>();
            parameters.QueryParameters = context.queryParameter() != null ?
                (Dictionary<string, string>)Visit(context.queryParameter()) : new Dictionary<string, string>();

            return parameters;
        }

        public override object VisitQueryParameter([NotNull] GrammarParser.QueryParameterContext context)
        {
            var parameters = new Dictionary<string, string>();
            if (context == null)
                return parameters;

            foreach (var prop in context.propertyDefenition())
            {
                var property = (KeyValuePair<string, string>)Visit(prop);

                ShouldBeUnique("QueryParameter", property.Value, 
                    parameters.Select(x => x.Value), prop);

                parameters.Add(property.Key, property.Value);
            }

            return parameters;
        }

        public override object VisitBodyParameter([NotNull] GrammarParser.BodyParameterContext context)
        {
            if (context == null)
                return new KeyValuePair<string, string>();

            var keyValue = (KeyValuePair<string, string>)Visit(context.propertyDefenition());
            return keyValue;
        }

        public override object VisitAttributes([NotNull] GrammarParser.AttributesContext context)
        {
            var attributes = new Dictionary<string, string>();

            if (context == null)
                return attributes;

            foreach(var attribute in context.attribute())
            {
                var keyValue = (KeyValuePair<string, string>)Visit(attribute);

                ShouldBeUnique("Attribute", keyValue.Key, 
                    attributes.Select(x => x.Key), attribute);

                attributes.Add(keyValue.Key, keyValue.Value);
            }

            return attributes;
        }

        public override object VisitAttribute([NotNull] GrammarParser.AttributeContext context)
        {
            if (context == null)
                return new KeyValuePair<string, string>();

            
            var keyValue = new KeyValuePair<string, string>(context.key.Text, context.value.Text);

            if (!_supportedAttributes.Contains(keyValue.Key))
            {
                throw new GrammarException($"Attribute {keyValue.Key} does not supported!")
                {
                    FromLine = context.SourceInterval.a,
                    ToLine = context.SourceInterval.b
                };
            }

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
