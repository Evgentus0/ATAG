using Antlr4.Runtime;
using ATAG.Core.Interfaces;
using ATAG.Core.Models;
using ATAG.Core.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core
{
    public class MainParser : IMainParser
    {
        private MainVisitor _mainVisitor;

        public MainParser(MainVisitor mainVisitor)
        {
            _mainVisitor = mainVisitor;
        }

        public FileParseResult ParseProtoFile(string content)
        {
            var lexer = new GrammarLexer(new AntlrInputStream(content));
            lexer.RemoveErrorListeners();
            var tokens = new CommonTokenStream(lexer);
            var parser = new GrammarParser(tokens);

            var tree = parser.instructions();
            var result = _mainVisitor.Visit(tree);

            return (FileParseResult)result;
        }
    }
}
