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
        public string? Id { get; }
        public string Name { get; }
        public string HexStr { get => string.Concat(Bytes.Select(b => b.ToString("x2"))); }
        public byte[] Bytes { get; }
        public Field(string? id, string name, params byte[] data)
        {
            Id = id;
            Bytes = data;
            Name = name;
        }
    }
}
