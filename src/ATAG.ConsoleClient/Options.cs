using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.ConsoleClient
{
    public class Options
    {
        [Option('s', "source", Required = true, HelpText = "Path to the source file with \"atag\" extension")]
        public string Source { get; set; }
        [Option('d', "dest", Required = true, HelpText = "Path to directory where geenrated files will be located")]
        public string Destination { get; set; }
        [Option('t', "type", Required = true, HelpText = "The type of generated files. Has three options: BackEnd, CSharpClient, AngularClient")]
        public string Type { get; set; }
        [Option('n', "namespace", Required = false, HelpText = "The Namespace where files will be generated")]
        public string Namespace { get; set; } = "AtagGenerated";
    }
}
