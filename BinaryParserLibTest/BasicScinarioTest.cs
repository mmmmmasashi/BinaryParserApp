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

    [Fact]
    public void 複数フィールドある場合も読めること()
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
    public void repeat機能で同じブロックを固定数繰り返すこと()
    {
        var result = ParseBySettingAndBin("010_repeat.json", "010_repeat.bin");
        Assert.Equal(2 + 3, result.RootFields.Count);

        //"1field", サイズ1が2回繰り返される
        Assert.Equal("01", result.RootFields[0].HexStr);
        Assert.Equal("1field(1)", result.RootFields[0].Name);

        Assert.Equal("02", result.RootFields[1].HexStr);
        Assert.Equal("1field(2)", result.RootFields[1].Name);

        //"2fields", サイズ2が3回繰り返される
        Assert.Equal("0304", result.RootFields[2].HexStr);
        Assert.Equal("2fields(1)", result.RootFields[2].Name);

        Assert.Equal("0506", result.RootFields[3].HexStr);
        Assert.Equal("2fields(2)", result.RootFields[3].Name);

        Assert.Equal("0708", result.RootFields[4].HexStr);
        Assert.Equal("2fields(3)", result.RootFields[4].Name);
    }

    [Fact]
    public void 別フィールドで指定した数値をもとにブロック数を繰り返す()
    {
        var result = ParseBySettingAndBin("011_repeat_by_field.json", "011_repeat_by_field.bin");
        Assert.Equal(1 + 3, result.RootFields.Count);

        Assert.Equal("block-num", result.RootFields[0].Name);
        Assert.Equal("0300", result.RootFields[0].HexStr);

    }
}