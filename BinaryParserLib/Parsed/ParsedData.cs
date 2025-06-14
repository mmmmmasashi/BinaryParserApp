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

        public ParsedData(string? protocolName, params Field[] fields)
        {
            ProtocolName = protocolName;
            RootFields = fields.ToList();
        }
    }
}
