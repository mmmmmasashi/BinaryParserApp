using BinaryParserLib.Parsed;
using BinaryParserLib.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Parser
{
    public class BinaryParser
    {
        private ProtocolSetting setting;

        public BinaryParser(ProtocolSetting setting)
        {
            this.setting = setting;
        }

        internal ParsedData ParseBinaryFile(string filePath)
        {
            var binData = File.ReadAllBytes(filePath);
            var rootField = this.setting.Structure.First();
            var firstFieldData = Enumerable.Range(0, rootField.Size ?? 1).Select(idx => binData[idx]);
            return new ParsedData(setting.ProtocolName, new Field(rootField.Name, firstFieldData.ToArray()));
        }
    }
}
