using BinaryParserLib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinaryParserLib.Protocol;

public class ProtocolSetting
{
    public static ProtocolSetting FromJsonFile(string filePath)
    {
        filePath = PathUtil.RemoveDoubleQuatation(filePath);

        var jsonContent = File.ReadAllText(filePath);
        var settingRaw = System.Text.Json.JsonSerializer.Deserialize<ProtocolSetting>(jsonContent) 
            ?? throw new InvalidOperationException("Failed to deserialize ProtocolSetting from JSON file.");

        return settingRaw;
    }

    [JsonPropertyName("protocolName")]
    public string? ProtocolName { get; init; }

    
    [JsonPropertyName("structure")]
    public List<FieldSetting> Structure { get; init; } = new List<FieldSetting>();

}
