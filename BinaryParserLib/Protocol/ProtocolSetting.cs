using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinaryParserLib.Protocol
{
    public class ProtocolSetting
    {
        public static ProtocolSetting FromJsonFile(string filePath)
        {
            var jsonContent = File.ReadAllText(filePath);
            return System.Text.Json.JsonSerializer.Deserialize<ProtocolSetting>(jsonContent) 
                ?? throw new InvalidOperationException("Failed to deserialize ProtocolSetting from JSON file.");
        }

        [JsonPropertyName("protocolName")]
        public string? ProtocolName { get; set; }

        
        [JsonPropertyName("structure")]
        public List<FieldSetting> Structure { get; set; } = new List<FieldSetting>();
    }
}
