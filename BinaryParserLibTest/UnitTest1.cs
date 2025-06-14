using BinaryParserLib.Parsed;
using BinaryParserLib.Parser;
using BinaryParserLib.Protocol;
using System.Text.Json;

namespace BinaryParserLibTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            ProtocolSetting setting = ProtocolSetting.FromJsonFile(Constants.GetPathOf("001_minset.json"));
            BinaryParser parser = new BinaryParser(setting);
            ParsedData result = parser.ParseBinaryFile(Constants.GetPathOf("001_min.bin"));
            
            Assert.Equal("Protocol X", result.ProtocolName);

        }
    }
}