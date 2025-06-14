using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BinaryParserLib.Parsed;

public class Field
{
    public string? Id { get; }
    public string Name { get; }
    public string HexStr { get => string.Concat(Bytes.Select(b => b.ToString("x2"))); }
    public byte[]? Bytes { get; }
    public List<Field> Children { get; }

    public Field(string? id, string name, byte[]? data, List<Field>? children = null)
    {
        Id = id;
        Bytes = data;
        Name = name;
        Children = new List<Field>();
        if (children is not null) Children.AddRange(children);
    }

    internal static Field CreateBlock(string? id, string name, List<Field> children)
    {
        return new Field(id, name, null, children);
    }
}
