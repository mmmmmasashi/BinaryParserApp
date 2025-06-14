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
        var jsonContent = File.ReadAllText(filePath);
        var settingRaw = System.Text.Json.JsonSerializer.Deserialize<ProtocolSetting>(jsonContent) 
            ?? throw new InvalidOperationException("Failed to deserialize ProtocolSetting from JSON file.");

        return settingRaw.ExpandFieldsWithRepeatCount();
    }

    [JsonPropertyName("protocolName")]
    public string? ProtocolName { get; init; }

    
    [JsonPropertyName("structure")]
    public List<FieldSetting> Structure { get; init; } = new List<FieldSetting>();

    private ProtocolSetting ExpandFieldsWithRepeatCount()
    {
        return new ProtocolSetting
        {
            ProtocolName = this.ProtocolName,
            Structure = this.Structure.SelectMany(field =>
            {
                int? repeatCount = null;
                
                if (field.Repeat.HasValue && field.Repeat.Value > 1)
                {
                    repeatCount = field.Repeat.Value;
                }

                return repeatCount is null? new List<FieldSetting> { field }:
                        Enumerable.Range(0, repeatCount.Value).Select(i => field.CopyUsingNumber(i + 1));

            }).ToList()
        };
    }

}
