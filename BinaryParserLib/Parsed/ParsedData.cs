using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Parsed
{
    public class ParsedData
    {
        public string? ProtocolName { get; }
        public List<Field> RootFields { get; internal set; }

        public ParsedData()
        {
            ProtocolName = "Protocol X";
            RootFields = new List<Field>() { new Field() };
        }
    }
}
