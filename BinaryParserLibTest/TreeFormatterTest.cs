using BinaryParserLib.Parsed;
using BinaryParserLib.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BinaryParserLibTest;

public class TreeFormatterTest
{
    private readonly ITestOutputHelper _output;

    public TreeFormatterTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void マルチセンサーデータのパース()
    {
        var result = BasicScenarioTest.ParseBySettingAndBin("024_multi_sensor_sample.json", "024_multi_sensor_sample.bin");

        var lines = new ParsedDataConverter().FormatToTsv(result);
        
        //foreach (var line in lines)
        //{
        //    _output.WriteLine(line);
        //}

        int i = 0;
        Assert.Equal("センサー数\t\t02", lines[i++]);
        Assert.Equal("センサーブロック(1)\tセンサーID\t01", lines[i++]);
        Assert.Equal("センサーブロック(1)\t値\t1234", lines[i++]);
        Assert.Equal("センサーブロック(2)\tセンサーID\t02", lines[i++]);
        Assert.Equal("センサーブロック(2)\t値\t5678", lines[i++]);
    }

    [Fact]
    public void タブ区切りまではせずに_整形しやすいデータ構造に変換するところまで変換する()
    {
        var result = BasicScenarioTest.ParseBySettingAndBin("024_multi_sensor_sample.json", "024_multi_sensor_sample.bin");

        var data = new ParsedDataConverter().ConvertToTableData(result);
        
        //タイトル
        Assert.Equal("マルチセンサープロトコル", data.ProtocolName);

        //テーブルの中身
        int i = 0;
        Assert.Equal(new List<string> { "センサー数",          "",          "02"},   data.Rows[i++]);
        Assert.Equal(new List<string> { "センサーブロック(1)", "センサーID","01"},   data.Rows[i++]);
        Assert.Equal(new List<string> { "センサーブロック(1)", "値",        "1234"}, data.Rows[i++]);
        Assert.Equal(new List<string> { "センサーブロック(2)", "センサーID","02"},   data.Rows[i++]);
        Assert.Equal(new List<string> { "センサーブロック(2)", "値",        "5678" },data.Rows[i++]);
    }

}
