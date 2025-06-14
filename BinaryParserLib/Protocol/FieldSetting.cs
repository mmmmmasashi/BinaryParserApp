using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinaryParserLib.Protocol
{
    public class FieldSetting
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("type")]
        public string Type { get; set; } = "int";

        [JsonPropertyName("size")]
        public int? Size { get; set; } = 1;


        public FieldSetting() { }
        public FieldSetting(string name, string type, int size)
        {
            Name = name;
            Type = type;
            Size = size;
        }
    }
}
