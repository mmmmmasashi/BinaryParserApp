using System;

namespace BinaryParserLib.Parser;

public class HexStringReader : IBinaryReader, IDisposable
{
    private readonly string _text;
    private int _position;
    
    public HexStringReader(string text)
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

    public void Dispose()
    {
        // Nothing to dispose
    }
}