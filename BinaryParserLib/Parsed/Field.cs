using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Parsed
{
    public class Field
    {
        public string Name { get; }
        public string HexStr { get; }

        public Field()
        {
            Name = "Hello byte!";
            HexStr = "00";
        }
    }
}
