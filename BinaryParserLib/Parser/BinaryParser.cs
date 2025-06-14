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
            
            var idxCurrent = 0;
            var fields = this.setting.Structure.Select(fieldSetting =>
            {
                var fieldSize = fieldSetting.Size ?? 1;
                var fieldData = Enumerable.Range(0, fieldSize).Select(offset => binData[idxCurrent + offset]).ToArray();

                idxCurrent += fieldSize;

                return new Field(fieldSetting.Name, fieldData);
            });

            return new ParsedData(this.setting.ProtocolName, fields.ToArray());
        }
    }
}
