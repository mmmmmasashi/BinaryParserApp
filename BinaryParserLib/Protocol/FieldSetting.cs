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
        public string Type { get; set; } = "bytes";

        [JsonPropertyName("size")]
        public int? Size { get; set; } = 1;

        [JsonPropertyName("repeat")]
        public int? Repeat { get; set; } = null;


        public FieldSetting() { }
        public FieldSetting(string name, string type, int size)
        {
            Name = name;
            Type = type;
            Size = size;
        }

        internal FieldSetting CopyUsingNumber(int number)
        {
            return new FieldSetting
            {
                Name = $"{this.Name}({number})",
                Size = this.Size,
                Type = this.Type
            };
        }
    }
}
