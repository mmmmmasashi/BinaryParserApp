using BinaryParserLib.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BinaryParserLib.Parsed;

public class Field : ITreeNode<Field>
{
    public string? Id { get; }
    public string Name { get; }
    public string HexStr { get => (Bytes is null)? "--" : string.Concat(Bytes.Select(b => b.ToString("X2"))); }
    public byte[]? Bytes { get; }
    public List<Field> Children { get; }

    public string ToLine()
    {
        //Name, HexStrをつなげて出力
        string namePart = string.IsNullOrEmpty(Name) ? "--" : Name;
        string hexPart = string.IsNullOrEmpty(HexStr) ? "--" : HexStr;
        return $"{namePart} : {hexPart}";
    }

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

    internal int ParseToInt()
    {
        if (this.Bytes is null) throw new InvalidOperationException("Bytes is null.");
        if (Bytes.Length == 1) return BitConverter.ToInt16(new byte[] { Bytes[0], 0x00 });
        return BitConverter.ToInt16(Bytes);
    }
}
