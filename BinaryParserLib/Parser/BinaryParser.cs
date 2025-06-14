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
            var data = Enumerable.Range(0, this.setting.Structure.First().Size ?? 1).Select(idx => binData[idx]);
            return new ParsedData(setting.ProtocolName, new Field(this.setting.Structure.First().Name, data.ToArray()));
        }
    }
}
