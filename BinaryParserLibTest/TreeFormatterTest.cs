using BinaryParserLib.Parsed;
using BinaryParserLib.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        var lines = new ParsedDataToTsvFormatter().Format(result);
        
        //foreach (var line in lines)
        //{
        //    _output.WriteLine(line);
        //}

        int i = 0;
        Assert.Equal("マルチセンサープロトコル\tセンサー数\t\t02", lines[i++]);
        Assert.Equal("マルチセンサープロトコル\tセンサーブロック(1)\tセンサーID\t01", lines[i++]);
        Assert.Equal("マルチセンサープロトコル\tセンサーブロック(1)\t値\t1234", lines[i++]);
        Assert.Equal("マルチセンサープロトコル\tセンサーブロック(2)\tセンサーID\t02", lines[i++]);
        Assert.Equal("マルチセンサープロトコル\tセンサーブロック(2)\t値\t5678", lines[i++]);

    }

}
