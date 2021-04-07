using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Exceptions
{
    public class GrammarException: Exception
    {
        public int FromLine { get; set; }
        public int ToLine { get; set; }

        public GrammarException(string message): base(message)
        { }
        public GrammarException(string message, Exception ex) : base(message, ex)
        { }

        public GrammarException()
        { }
    }
}
