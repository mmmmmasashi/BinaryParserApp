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
        //TODO: ファイルパス指定とファイル内容指定は別経路に分けるが、一時的にここで対応する
        IBinaryReader reader;
        if (File.Exists(filePath))
        {
            reader = new MyBinaryReader(new MemoryStream(File.ReadAllBytes(filePath)));
        }
        else
        {
            var text = filePath;
            reader = new HexTextReader(text);
        }

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


    //TODO:別ファイル化？整理する
    class MyBinaryReader : IBinaryReader
    {
        private readonly MemoryStream _stream;
        private readonly BinaryReader _reader;
        public MyBinaryReader(MemoryStream stream)
        {
            _stream = stream;
            _reader = new BinaryReader(stream);
        }
        public byte[] ReadBytes(int count)
        {
            return _reader.ReadBytes(count);
        }
        public void Dispose()
        {
            _reader.Dispose();
            _stream.Dispose();
        }
    }

    //TODO:別ファイル化？整理する
    class HexTextReader : IBinaryReader
    {
        private readonly string _text;
        private int _position;
        public HexTextReader(string text)
        {
            _text = text.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("-", "").Replace("_", "");
            _position = 0;
        }
        public byte[] ReadBytes(int count)
        {
            if (_position + count * 2 > _text.Length)
            {
                throw new ArgumentOutOfRangeException("Not enough data to read.");
            }
            var bytes = new byte[count];
            for (int i = 0; i < count; i++)
            {
                bytes[i] = Convert.ToByte(_text.Substring(_position, 2), 16);
                _position += 2;
            }
            return bytes;
        }
    }
}
