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
    public int? Repeat { get; set; } = null;

    [JsonPropertyName("repeatById")]
    public string? RepeatById { get; set; }

    [JsonPropertyName("content")]
    public List<FieldSetting> Content { get; init; } = new List<FieldSetting>();

    public int ByteSize{
        get
        {
            if (Type == "uint16") return 2;
            if (Type == "uint8") return 1;
            if (Size.HasValue) return Size.Value;
            throw new InvalidDataException("Field size is not defined.");
        }
    }


    internal FieldSetting RenameByRepeat(int number)
    {
        var copy = (FieldSetting)this.MemberwiseClone();
        copy.Name = $"{this.Name}({number})";
        copy.Repeat = null;
        copy.RepeatById = null;
        return copy;
    }
}
