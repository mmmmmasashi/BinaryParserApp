using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinaryParserLib.Protocol;

public class FieldSetting
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("type")]
    public string Type { get; init; } = "bytes";

    //データ型によって決まることもあるので直接アクセスしない

    [JsonPropertyName("size")]
    public int? Size { get; init; } = 1;

    [JsonPropertyName("repeat")]
    public int? Repeat { get; init; } = null;

    [JsonPropertyName("repeatById")]
    public string? RepeatById { get; init; }

    public int ByteSize{
        get
        {
            if (Type == "uint16") return 2;
            if (Size.HasValue) return Size.Value;
            throw new InvalidDataException("Field size is not defined.");
        }
    }


    public FieldSetting() { }


    internal FieldSetting CopyUsingNumber(int number)
    {
        var copy = (FieldSetting)this.MemberwiseClone();
        copy.Name = $"{this.Name}({number})";
        return copy;
    }
}
