using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BinaryParserLib.Parsed
{
    public class Field
    {
        public string Name { get; }
        public string HexStr { get => string.Concat(bytes.Select(b => b.ToString("x2"))); }
        public byte[] bytes { get; }
        public Field(string name, params byte[] data)
        {
            bytes = data;
            Name = name;
        }
    }
}
