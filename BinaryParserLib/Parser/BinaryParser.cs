using BinaryParserLib.Common;
using BinaryParserLib.Parsed;
using BinaryParserLib.Protocol;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserLib.Parser;

public class BinaryParser(ProtocolSetting setting, CancellationToken? token = null)
{
    private readonly CancellationToken? _token = token;
    private readonly ProtocolSetting _setting = setting;

    public ParsedData ParseBinaryFile(string filePath)
    {
        filePath = PathUtil.RemoveDoubleQuatation(filePath);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("指定されたファイルが見つかりません。", filePath);
        }

        using var reader = new FileBinaryReader(new MemoryStream(File.ReadAllBytes(filePath)));
        return Parse(reader);
    }

    public ParsedData ParseBinaryString(string hexString)
    {
        using var reader = new HexStringReader(hexString);
        return Parse(reader);
    }

    private ParsedData Parse(IBinaryReader reader)
    {
        var fieldList = new List<Field>();

        foreach (var eachRawSetting in _setting.Structure)
        {
            if (_token != null && _token.HasValue && _token.Value.IsCancellationRequested)
            {
                throw new OperationCanceledException("Parsing was canceled by the user.");
            }
            new FieldParser().ParseField(reader, eachRawSetting, fieldList);
        }

        return new ParsedData(_setting.ProtocolName, fieldList.ToArray());
    }


}
