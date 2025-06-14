using BinaryParserLib.Parsed;
using BinaryParserLib.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Parser;

internal class FieldParser
{
    internal void ParseField(BinaryReader reader, FieldSetting setting, List<Field> fieldListCurrent)
    {
        //ブロックの場合
        if (setting.Type is string type && type == "block")
        {
            ParseBlockField(reader, setting, fieldListCurrent);
        }
        else
        {
            ParseNonBlockField(reader, setting, fieldListCurrent);
        }
    }

    private void ParseNonBlockField(BinaryReader reader, FieldSetting setting, List<Field> fieldListCurrent)
    {
        //固定サイズの場合
        if (setting.Repeat is int repeatFixedCount && repeatFixedCount > 0)
        {
            ParseFixedSizeRepeatFields(reader, setting, fieldListCurrent, repeatFixedCount);
        }
        //可変サイズの場合
        else if (setting.RepeatById is string repeatById)
        {
            ParseVariableSizeRepeatFields(reader, setting, fieldListCurrent, repeatById);
        }
        else
        {
            ParseSingleField(reader, setting, fieldListCurrent);
        }
    }

    private void ParseBlockField(BinaryReader reader, FieldSetting setting, List<Field> fieldListCurrent)
    {
        var content = setting.Content;
        if (content == null) throw new InvalidDataException();//空のブロック？

        //固定サイズリピートの場合
        if (setting.Repeat is int repeatFixedCount && repeatFixedCount > 0)
        {
            //ブロック名をリネームしてコピー
            ParseCopiedBlock(reader, setting, fieldListCurrent, repeatFixedCount);
        }
        //可変サイズリピートの場合
        else if (setting.RepeatById is string repeatById)
        {
            int repeatCount = FindRepeatCount(fieldListCurrent, repeatById);
            ParseCopiedBlock(reader, setting, fieldListCurrent, repeatCount);
        }
        else
        {
            //1つのブロックが渡される

            //ブロック内の各フィールドを再帰的に解析
            var children = new List<Field>();
            foreach (var eachSetting in content)
            {
                ParseNonBlockField(reader, eachSetting, children);
            }

            //それを子にもつブロックフィールドを一つ作り、渡されたリストに並列に追加する
            var blockField = Field.CreateBlock(setting.Id, setting.Name, children);
            fieldListCurrent.Add(blockField);
        }
    }

    private void ParseCopiedBlock(BinaryReader reader, FieldSetting setting, List<Field> fieldListCurrent, int repeatCount)
    {
        foreach (var number in Enumerable.Range(1, repeatCount))
        {
            var copiedBlockSetting = setting.RenameByRepeat(number);
            ParseField(reader, copiedBlockSetting, fieldListCurrent);
        }
    }

    private static void ParseSingleField(BinaryReader reader, FieldSetting setting, List<Field> fieldListCurrent)
    {
        var byteSize = setting.ByteSize;
        var fieldData = reader.ReadBytes(byteSize).ToArray();
        fieldListCurrent.Add(new Field(setting.Id, setting.Name, fieldData));
    }

    private void ParseVariableSizeRepeatFields(BinaryReader reader, FieldSetting setting, List<Field> fieldListCurrent, string repeatById)
    {
        int repeatCount = FindRepeatCount(fieldListCurrent, repeatById);

        var expandedSettingList = Enumerable.Range(1, repeatCount).Select(number => setting.RenameByRepeat(number)).ToList();
        var ans = new List<Field>();
        foreach (var eachSetting in expandedSettingList)
        {
            ParseField(reader, eachSetting, fieldListCurrent);
        }
    }

    private static int FindRepeatCount(List<Field> fieldListCurrent, string repeatById)
    {
        var repeatField = fieldListCurrent.FirstOrDefault(f => f?.Id == repeatById);
        if (repeatField == null)
        {
            throw new InvalidOperationException($"Repeat field '{repeatById}' not found in parsed fields.");
        }

        //TODO:違う階層にID指定があった場合に取れない？
        return repeatField.ParseToInt();
    }

    private void ParseFixedSizeRepeatFields(BinaryReader reader, FieldSetting setting, List<Field> fieldListCurrent, int repeatFixedCount)
    {
        for (int i = 0; i < repeatFixedCount; i++)
        {
            var expandedSetting = setting.RenameByRepeat(i + 1);
            ParseField(reader, expandedSetting, fieldListCurrent);
        }
    }
}
