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

        // 残りのデータがある場合は「未定義」フィールドとして追加
        try
        {
            var buffer = new List<byte>();
            while (true)
            {
                var data = reader.ReadBytes(1);
                if (data.Length == 0) break;
                buffer.AddRange(data);
            }

            if (buffer.Count > 0)
            {
                fieldList.Add(new Field(null, "undefined", buffer.ToArray()));
            }
        }
        catch (Exception)
        {
            // データの終端に達した場合は正常終了
        }

        return new ParsedData(_setting.ProtocolName, fieldList.ToArray());
    }


}
