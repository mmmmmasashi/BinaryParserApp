using System;
using System.IO;

namespace BinaryParserLib.Parser;

public class FileBinaryReader : IBinaryReader, IDisposable
{
    private readonly MemoryStream _stream;
    private readonly BinaryReader _reader;
    
    public FileBinaryReader(MemoryStream stream)
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