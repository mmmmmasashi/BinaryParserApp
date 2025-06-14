using BinaryParserLib.Parsed;
using BinaryParserLib.Protocol;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            var fieldList = new List<Field>();
            var idxCurrent = 0;

            foreach (var eachRawSetting in this.setting.Structure)
            {
                List<FieldSetting> expandedSettings = ExpandRepeatById(eachRawSetting, fieldList);
                foreach (var eachSetting in expandedSettings)
                {
                    var byteSize = eachSetting.ByteSize;
                    var fieldData = binData.Skip(idxCurrent).Take(byteSize).ToArray();
                    idxCurrent += byteSize;
                    fieldList.Add(new Field(eachSetting.Id, eachSetting.Name, fieldData));

                }
            }

            return new ParsedData(this.setting.ProtocolName, fieldList.ToArray());
        }

        private List<FieldSetting> ExpandRepeatById(FieldSetting eachRawSetting, List<Field> fieldList)
        {
            //可変サイズの場合
            if (eachRawSetting.RepeatById != null)
            {
                var repeatField = fieldList.FirstOrDefault(f => f?.Id == eachRawSetting.RepeatById);
                if (repeatField == null)
                {
                    throw new InvalidOperationException($"Repeat field '{eachRawSetting.RepeatById}' not found in parsed fields.");
                }
                //符号なし16bitのバイト列を前提に解釈
                int repeatCount =  BitConverter.ToInt16(repeatField.Bytes);

                return Enumerable.Range(1, repeatCount).Select(number => eachRawSetting.CopyUsingNumber(number)).ToList();
            }

            return new List<FieldSetting>() { eachRawSetting };
        }

    }
}
