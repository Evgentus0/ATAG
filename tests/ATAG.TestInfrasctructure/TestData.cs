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
	
	[""test_fdfted_sf""] get MethodName1() return string;
    post MethodName2(fromBody: string StrParameter; fromQuery: int qpInt, string qpStr) return string;
    [""test-route{1}/wer""] put MethodName3(fromBody: Model1 m1) return int;
}

model Model1{
    string Str1;
    int Int1;
}

model Test_Model{
    int m2int2;
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
                                    Route = "test_fdf_tedsf",
                                    Verb = Core.Models.Enums.HttpVerb.get,
                                    Name = "MethodName1",
                                    ReturnedType = "string",
                                    Parameters = new ParameterModel()
                                },
                                new MethodModel
                                {
                                    Route = string.Empty,
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
                                },
                                new MethodModel
                                {
                                    Name="MethodName3",
                                    Route="test-route{1}/wer",
                                    Verb = Core.Models.Enums.HttpVerb.put,
                                    ReturnedType="int",
                                    Parameters = new ParameterModel
                                    {
                                        BodyParameter = new KeyValuePair<string, string>("Model1", "m1")
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
                        },
                        new EntityModel
                        {
                            Name = "Test_Model",
                            Properties = new Dictionary<string, string>
                            {
                                ["int"] = "m2int2"
                            }
                        }
                    }
                }
            );
    }
}
