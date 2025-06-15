// See https://aka.ms/new-console-template for more information
using BinaryParserLib.Parsed;
using BinaryParserLib.Parser;
using BinaryParserLib.Protocol;
using BinaryParserLib.Text;


//binfile.bin : バイナリファイル 第一引数で指定
//setting.json : 設定のjsonファイル 第二引数で指定


string binFile = (args.Length >= 1)? args[0] : "Sample/sample-data.bin";
string settingFile= (args.Length >= 2) ? args[1] : "Sample/sample-setting.json";

try
{
    ProtocolSetting setting = ProtocolSetting.FromJsonFile(settingFile);
    BinaryParser parser = new BinaryParser(setting);
    ParsedData result = parser.ParseBinaryFile(binFile);
    Console.WriteLine($"Protocol Name: {result.ProtocolName ?? "-" }");

    var lines = new ParsedDataConverter().FormatToTsv(result);
    foreach (var line in lines)
    {
        Console.WriteLine(line);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.Write(ex.StackTrace);
}