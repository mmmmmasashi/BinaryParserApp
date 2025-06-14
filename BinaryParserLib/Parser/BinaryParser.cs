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
            return new ();
        }
    }
}
