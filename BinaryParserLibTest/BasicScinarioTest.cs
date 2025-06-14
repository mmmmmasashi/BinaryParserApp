using BinaryParserLib.Parsed;
using BinaryParserLib.Parser;
using BinaryParserLib.Protocol;
using System.Text.Json;

namespace BinaryParserLibTest
{
    public class BasicScinarioTest
    {
        [Fact]
        public void 基本的なバイナリ解析シナリオ()
        {
            ProtocolSetting setting = ProtocolSetting.FromJsonFile(Constants.GetPathOf("001_minset.json"));
            BinaryParser parser = new BinaryParser(setting);
            ParsedData result = parser.ParseBinaryFile(Constants.GetPathOf("001_min.bin"));
            
            Assert.Equal("Protocol X", result.ProtocolName);
            Assert.Single(result.RootFields);
            var field = result.RootFields[0];

            Assert.Equal("Hello byte!", field.Name);
            Assert.Equal("00", field.HexStr);
        }
    }
}