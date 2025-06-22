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

    private ParsedData result;

    public TreeFormatterTest(ITestOutputHelper output)
    {
        _output = output;
        result = BasicScenarioTest.ParseBySettingAndBin("024_multi_sensor_sample.json", "024_multi_sensor_sample.bin");
    }

    [Fact]
    public void サンプル1_Numberオプションあり()
    {
        var opt = new TableFormatOption
        {
            UseNumberOption = true
        };


        var lines = new ParsedDataConverter(opt).FormatToTsv(result);

        //foreach (var line in lines)
        //{
        //    _output.WriteLine(line);
        //}

        int i = 0;
        Assert.Equal("1\tセンサー数\t\t02", lines[i++]);
        Assert.Equal("2\tセンサーブロック(1)\tセンサーID\t01", lines[i++]);
        Assert.Equal("3\tセンサーブロック(1)\t値\t1234", lines[i++]);
        Assert.Equal("4\tセンサーブロック(2)\tセンサーID\t02", lines[i++]);
        Assert.Equal("5\tセンサーブロック(2)\t値\t5678", lines[i++]);

    }


    [Fact]
    public void サンプル1()
    {
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

    [Fact]
    public void Numberつきの場合()
    {
        var option = new TableFormatOption
        {
            UseNumberOption = true
        };
        var data = new ParsedDataConverter(option).ConvertToTableData(result);

        //タイトル
        Assert.Equal("マルチセンサープロトコル", data.ProtocolName);

        //ヘッダー
        Assert.Equal(new List<string> { "No.", "h1", "h2", "data(HEX)" }, data.GetHeaderNames());

        //テーブルの中身
        int i = 0;
        Assert.Equal(new List<string> { "1", "センサー数", "", "02" }, data.Rows[i++]);
        Assert.Equal(new List<string> { "2", "センサーブロック(1)", "センサーID", "01" }, data.Rows[i++]);
        Assert.Equal(new List<string> { "3", "センサーブロック(1)", "値", "1234" }, data.Rows[i++]);
        Assert.Equal(new List<string> { "4", "センサーブロック(2)", "センサーID", "02" }, data.Rows[i++]);
        Assert.Equal(new List<string> { "5", "センサーブロック(2)", "値", "5678" }, data.Rows[i++]);
    }


    [Fact]
    public void Number_Index表示つきの場合()
    {
        var option = new TableFormatOption
        {
            UseNumberOption = true,
            ShowIndex = true
        };
        var data = new ParsedDataConverter(option).ConvertToTableData(result);

        //タイトル
        Assert.Equal("マルチセンサープロトコル", data.ProtocolName);

        //ヘッダー
        Assert.Equal(new List<string> { "No.", "h1", "h2", "index", "data(HEX)" }, data.GetHeaderNames());

        //テーブルの中身
        int i = 0;
        Assert.Equal(new List<string> { "1", "センサー数", "", "0", "02" }, data.Rows[i++]);
        Assert.Equal(new List<string> { "2", "センサーブロック(1)", "センサーID", "1", "01" }, data.Rows[i++]);
        Assert.Equal(new List<string> { "3", "センサーブロック(1)", "値", "2", "1234" }, data.Rows[i++]);
        Assert.Equal(new List<string> { "4", "センサーブロック(2)", "センサーID", "4", "02" }, data.Rows[i++]);
        Assert.Equal(new List<string> { "5", "センサーブロック(2)", "値", "5", "5678" }, data.Rows[i++]);
    }

}
