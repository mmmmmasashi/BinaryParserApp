using BinaryParserLib.Parsed;
using BinaryParserLib.Parser;
using BinaryParserLib.Protocol;
using System.Text.Json;

namespace BinaryParserLibTest
{
    public class BasicScinarioTest
    {
        private static ParsedData ParseBySettingAndBin(string settingFile, string binFile)
        {
            ProtocolSetting setting = ProtocolSetting.FromJsonFile(Constants.GetPathOf(settingFile));
            BinaryParser parser = new BinaryParser(setting);
            return parser.ParseBinaryFile(Constants.GetPathOf(binFile));
        }

        [Fact]
        public void 基本的なバイナリ解析シナリオ1()
        {
            var result = ParseBySettingAndBin("001_minset.json", "001_min.bin");
            Assert.Equal("Protocol X", result.ProtocolName);
            Assert.Single(result.RootFields);
            var field = result.RootFields[0];

            Assert.Equal("Hello byte!", field.Name);
            Assert.Equal("00", field.HexStr);
        }

        [Fact]
        public void 基本的なバイナリ解析シナリオ2()
        {
            var result = ParseBySettingAndBin("002_minset.json", "002_min.bin");

            Assert.Equal("Protocol Y", result.ProtocolName);
            Assert.Single(result.RootFields);
            var field = result.RootFields[0];

            Assert.Equal("Hello bytes!", field.Name);
            Assert.Equal("0102", field.HexStr);
        }

    }
}