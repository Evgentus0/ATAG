using ATAG.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.TestInfrasctructure
{
    public class TestData
    {
        public static KeyValuePair<string, FileParseResult> CorrectControllerModel =
            new KeyValuePair<string, FileParseResult>
            (
@"
cntrl ControllerName1{
	[Route=test_fdf_tedsf] get MethodName1() return ResponseModelName1;
	post MethodName2(fromBody: string StrParameter; fromQuery: int qpInt, string qpStr) return ResponseModelName2;
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
                                    ReturnedType = "ResponseModelName1",
                                    Parameters = new ParameterModel()
                                },
                                new MethodModel
                                {
                                    Attributes = new Dictionary<string, string>(),
                                    Verb = Core.Models.Enums.HttpVerb.post,
                                    Name = "MethodName2",
                                    ReturnedType = "ResponseModelName2",
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
                    }
                }
            );
    }
}
