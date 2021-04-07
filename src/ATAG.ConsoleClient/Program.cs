using ATAG.Core.Factories;
using ATAG.Core.Models.Enums;
using ATAG.Core.Models.Inbound;
using CommandLine;
using System;

namespace ATAG.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { "-s", @"C:\Users\Evgentus\Desktop\New folder\sourceFile.atag",
            //    "--dest", @"C:\Users\Evgentus\Desktop\New folder\dest", "-t", "BackEnd", "-n", "TestNamespace" };

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    try
                    {
                        SupportedGenerators type = (SupportedGenerators)Enum.Parse(typeof(SupportedGenerators), o.Type);
                        var parameters = new InputParametersModel
                        {
                            DestinationPath = o.Destination,
                            GeneratorType = type,
                            SourceFilePath = o.Source,
                            Namespace = o.Namespace
                        };
                        var factory = new GeneratorFactory();
                        var generator = factory.GetGeneratorInstance(parameters);

                        generator.Execute();

                        Console.WriteLine("Generation is done");
                    }
                    catch(Exception ex)
                    {
                        Console.Beep();
                        Console.WriteLine("Error during generating files occures");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                })
                .WithNotParsed(errors =>
                {
                    Console.Beep();
                    Console.WriteLine("Incorrect input parameters!");
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error);
                    }
                });
        }
    }
}
