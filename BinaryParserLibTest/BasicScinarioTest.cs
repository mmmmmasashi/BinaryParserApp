using BinaryParserLib.Parsed;
using BinaryParserLib.Parser;
using BinaryParserLib.Protocol;
using System;
using System.Text.Json;
using Xunit;

namespace BinaryParserLibTest;

public class BasicScinarioTest
{
    private static ParsedData ParseBySettingAndBin(string settingFile, string binFile)
    {
        ProtocolSetting setting = ProtocolSetting.FromJsonFile(Constants.GetPathOf(settingFile));
        BinaryParser parser = new BinaryParser(setting);
        return parser.ParseBinaryFile(Constants.GetPathOf(binFile));
    }

    [Fact]
    public void 基本的なバイナリ入力シナリオ1()
    {
        var result = ParseBySettingAndBin("001_minset.json", "001_min.bin");
        Assert.Equal("Protocol X", result.ProtocolName);
        Assert.Single(result.RootFields);
        var field = result.RootFields[0];

        Assert.Equal("Hello byte!", field.Name);
        Assert.Equal("00", field.HexStr);
    }

    [Fact]
    public void 基本的なバイナリ入力シナリオ2()
    {
        var result = ParseBySettingAndBin("002_minset.json", "002_min.bin");

        Assert.Equal("Protocol Y", result.ProtocolName);
        Assert.Single(result.RootFields);
        var field = result.RootFields[0];

        Assert.Equal("Hello bytes!", field.Name);
        Assert.Equal("0102", field.HexStr);
    }

    [Fact]
    public void 複数フィールドがある場合が読める事()
    {
        var result = ParseBySettingAndBin("003_multi_fields.json", "003_multi_fields.bin");
        Assert.Equal("複数フィールドプロトコル", result.ProtocolName);
        Assert.Equal(2, result.RootFields.Count);

        var field1 = result.RootFields[0];
        Assert.Equal("1field", field1.Name);
        Assert.Equal("01", field1.HexStr);

        var field2 = result.RootFields[1];
        Assert.Equal("2fields", field2.Name);
        Assert.Equal("0203", field2.HexStr);
    }

    [Fact]
    public void repeat機能で同じブロックを固定数繰り返せること()
    {
        var result = ParseBySettingAndBin("010_repeat.json", "010_repeat.bin");
        Assert.Equal(2 + 3, result.RootFields.Count);

        //"1field", サイズ1を2回繰り返す
        Assert.Equal("01", result.RootFields[0].HexStr);
        Assert.Equal("1field(1)", result.RootFields[0].Name);

        Assert.Equal("02", result.RootFields[1].HexStr);
        Assert.Equal("1field(2)", result.RootFields[1].Name);

        //"2fields", サイズ2を3回繰り返す
        Assert.Equal("0304", result.RootFields[2].HexStr);
        Assert.Equal("2fields(1)", result.RootFields[2].Name);

        Assert.Equal("0506", result.RootFields[3].HexStr);
        Assert.Equal("2fields(2)", result.RootFields[3].Name);

        Assert.Equal("0708", result.RootFields[4].HexStr);
        Assert.Equal("2fields(3)", result.RootFields[4].Name);
    }

    [Fact]
    public void 別フィールドで指定した値だけブロックを繰り返す()
    {
        var result = ParseBySettingAndBin("011_repeat_by_field.json", "011_repeat_by_field.bin");
        Assert.Equal(1 + 3, result.RootFields.Count);

        Assert.Equal("block-num", result.RootFields[0].Name);
        Assert.Equal("0300", result.RootFields[0].HexStr);
    }

    [Fact]
    public void Blockの導入_ブロック数1()
    {
        var result = ParseBySettingAndBin("020_block.json", "020_block.bin");
        Assert.Equal(2, result.RootFields.Count);
        
        Assert.Equal("sample", result.RootFields[0].Name);
        Assert.Equal("00", result.RootFields[0].HexStr);

        Assert.Equal("blockName", result.RootFields[1].Name);
        var blockChildren = result.RootFields[1].Children;

        {
            Assert.Equal("1field", blockChildren[0].Name);
            Assert.Equal("2fields", blockChildren[1].Name);

            Assert.Equal("01", blockChildren[0].HexStr);
            Assert.Equal("0102", blockChildren[1].HexStr);
        }
    }

    [Fact]
    public void Blockの固定数繰り返し_ブロック数3()
    {
        var result = ParseBySettingAndBin("021_block_repeat.json", "021_block_repeat.bin");
        Assert.Equal(1 + 3, result.RootFields.Count);
        Assert.Equal("sample", result.RootFields[0].Name);
        Assert.Equal("00", result.RootFields[0].HexStr);

        Assert.Equal("blockName(1)", result.RootFields[1].Name);
        Assert.Equal("blockName(2)", result.RootFields[2].Name);
        Assert.Equal("blockName(3)", result.RootFields[3].Name);

        for (int i = 1; i <= 3; i++)
        {
            var blockChildren = result.RootFields[i].Children;
            {
                Assert.Equal(2, blockChildren.Count);
                Assert.Equal("1field", blockChildren[0].Name);
                Assert.Equal("2fields", blockChildren[1].Name);

                Assert.Equal("01", blockChildren[0].HexStr);
                Assert.Equal("0102", blockChildren[1].HexStr);
            }
        }
    }

    [Fact]
    public void Blockのフィールドに依存した繰り返し_ブロック数2()
    {
        var result = ParseBySettingAndBin("022_block_repeat_fieldsize.json", "022_block_repeat_fieldsize.bin");
        Assert.Equal(1 + 2, result.RootFields.Count);
        Assert.Equal("sample", result.RootFields[0].Name);
        Assert.Equal("02", result.RootFields[0].HexStr);

        Assert.Equal("blockName(1)", result.RootFields[1].Name);
        Assert.Equal("blockName(2)", result.RootFields[2].Name);

        for (int i = 1; i <= 2; i++)
        {
            var blockChildren = result.RootFields[i].Children;
            {
                Assert.Equal(2, blockChildren.Count);
                Assert.Equal("1field", blockChildren[0].Name);
                Assert.Equal("2fields", blockChildren[1].Name);

                Assert.Equal("01", blockChildren[0].HexStr);
                Assert.Equal("0102", blockChildren[1].HexStr);
            }
        }
    }

    [Fact]
    public void 基本センサーデータのパース()
    {
        var result = ParseBySettingAndBin("023_sensor_sample.json", "023_sensor_sample.bin");
        Assert.Equal("センサーデータプロトコル", result.ProtocolName);
        Assert.Equal(2, result.RootFields.Count);

        // デバイスIDの確認
        Assert.Equal("デバイスID", result.RootFields[0].Name);
        Assert.Equal("01", result.RootFields[0].HexStr);

        // センサー1ブロックの確認
        var sensorBlock = result.RootFields[1];
        Assert.Equal("センサー1", sensorBlock.Name);
        Assert.Equal(3, sensorBlock.Children.Count);

        // 温度の確認
        Assert.Equal("温度", sensorBlock.Children[0].Name);
        Assert.Equal("1234", sensorBlock.Children[0].HexStr);

        // 湿度の繰り返しの確認
        Assert.Equal("湿度(1)", sensorBlock.Children[1].Name);
        Assert.Equal("50", sensorBlock.Children[1].HexStr);

        Assert.Equal("湿度(2)", sensorBlock.Children[2].Name);
        Assert.Equal("60", sensorBlock.Children[2].HexStr);
    }

    [Fact]
    public void マルチセンサーデータのパース()
    {
        var result = ParseBySettingAndBin("024_multi_sensor_sample.json", "024_multi_sensor_sample.bin");
        Assert.Equal("マルチセンサープロトコル", result.ProtocolName);

        // センサー数の確認
        Assert.Equal("センサー数", result.RootFields[0].Name);
        Assert.Equal("02", result.RootFields[0].HexStr);

        // センサーブロック1の確認
        var block1 = result.RootFields[1];
        Assert.Equal("センサーブロック(1)", block1.Name);
        Assert.Equal("センサーID", block1.Children[0].Name);
        Assert.Equal("01", block1.Children[0].HexStr);
        Assert.Equal("値", block1.Children[1].Name);
        Assert.Equal("1234", block1.Children[1].HexStr);

        // センサーブロック2の確認
        var block2 = result.RootFields[2];
        Assert.Equal("センサーブロック(2)", block2.Name);
        Assert.Equal("センサーID", block2.Children[0].Name);
        Assert.Equal("02", block2.Children[0].HexStr);
        Assert.Equal("値", block2.Children[1].Name);
        Assert.Equal("5678", block2.Children[1].HexStr);
    }
}