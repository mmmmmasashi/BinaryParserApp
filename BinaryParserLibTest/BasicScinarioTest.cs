using BinaryParserLib.Parsed;
using BinaryParserLib.Parser;
using BinaryParserLib.Protocol;
using System;
using System.Text.Json;
using Xunit;

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
        public void ��{�I�ȃo�C�i����̓V�i���I1()
        {
            var result = ParseBySettingAndBin("001_minset.json", "001_min.bin");
            Assert.Equal("Protocol X", result.ProtocolName);
            Assert.Single(result.RootFields);
            var field = result.RootFields[0];

            Assert.Equal("Hello byte!", field.Name);
            Assert.Equal("00", field.HexStr);
        }

        [Fact]
        public void ��{�I�ȃo�C�i����̓V�i���I2()
        {
            var result = ParseBySettingAndBin("002_minset.json", "002_min.bin");

            Assert.Equal("Protocol Y", result.ProtocolName);
            Assert.Single(result.RootFields);
            var field = result.RootFields[0];

            Assert.Equal("Hello bytes!", field.Name);
            Assert.Equal("0102", field.HexStr);
        }

        [Fact]
        public void �����t�B�[���h����ꍇ���ǂ߂邱��()
        {
            var result = ParseBySettingAndBin("003_multi_fields.json", "003_multi_fields.bin");
            Assert.Equal("�����t�B�[���h�v���g�R��", result.ProtocolName);
            Assert.Equal(2, result.RootFields.Count);

            var field1 = result.RootFields[0];
            Assert.Equal("1field", field1.Name);
            Assert.Equal("01", field1.HexStr);

            var field2 = result.RootFields[1];
            Assert.Equal("2fields", field2.Name);
            Assert.Equal("0203", field2.HexStr);
        }

    }
}