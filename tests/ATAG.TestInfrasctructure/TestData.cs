using ATAG.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace ATAG.TestInfrasctructure
{
    public class TestData
    {
        public static KeyValuePair<string, FileParseResult> CorrectControllerModel =
            new KeyValuePair<string, FileParseResult>
            (
@"
cntrl ControllerName1{
	[Route=test_fdf_tedsf] get MethodName1() return string;
	post MethodName2(fromBody: string StrParameter; fromQuery: int qpInt, string qpStr) return string;
}

model Model1{
    string Str1;
    int Int1;
}
",
                new FileParseResult()
                {
                    Controllers = new List<ControllerModel>
                    {
                        new ControllerModel
                        {
                            Name = "ControllerName1",
                            Methods = new List<MethodModel>
                            {
                                new MethodModel
                                {
                                    Attributes = new Dictionary<string, string>{["Route"] = "test_fdf_tedsf"},
                                    Verb = Core.Models.Enums.HttpVerb.get,
                                    Name = "MethodName1",
                                    ReturnedType = "string",
                                    Parameters = new ParameterModel()
                                },
                                new MethodModel
                                {
                                    Attributes = new Dictionary<string, string>(),
                                    Verb = Core.Models.Enums.HttpVerb.post,
                                    Name = "MethodName2",
                                    ReturnedType = "string",
                                    Parameters = new ParameterModel
                                    {
                                        BodyParameter = new KeyValuePair<string, string>("string", "StrParameter"),
                                        QueryParameters = new Dictionary<string, string>
                                        {
                                            ["int"] = "qpInt",
                                            ["string"] = "qpStr"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Models = new List<EntityModel>
                    {
                        new EntityModel
                        {
                            Name = "Model1",
                            Properties = new Dictionary<string, string>
                            {
                                ["string"]="Str1",
                                ["int"]="Int1"
                            }
                        }
                    }
                }
            );
    }
}
