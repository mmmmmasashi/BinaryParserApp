using BinaryParserApp.View.Service;
using BinaryParserLib.Parsed;
using BinaryParserLib.Parser;
using BinaryParserLib.Protocol;
using BinaryParserLib.Text;
using Reactive.Bindings;
using System;
using System.IO;
using System.IO.Compression;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace BinaryParserApp.ViewModel
{
    public class MainWindowViewModel
    {
        private readonly IWindowService _windowService;

        // JSON設定ファイルパス
        public ReactiveProperty<string> JsonFilePath { get; set; }

        // BINファイルパス
        public ReactiveProperty<string> BinFilePath { get; set; }

        // 変換コマンド
        public ReactiveCommand ConvertCommand { get; }

        public MainWindowViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            // 設定から前回値を復元
            JsonFilePath = new ReactiveProperty<string>(Properties.Settings.Default.JsonFilePath);
            BinFilePath = new ReactiveProperty<string>(Properties.Settings.Default.BinFilePath);

            // 両方のパスが入力されているときだけボタンを有効化
            var canConvert = JsonFilePath
                .CombineLatest(BinFilePath, (json, bin) => !string.IsNullOrWhiteSpace(json) && !string.IsNullOrWhiteSpace(bin));

            ConvertCommand = canConvert.ToReactiveCommand();

            ConvertCommand.Subscribe(_ => Convert());
        }
        private void Convert()
        {
            // 設定に保存
            Properties.Settings.Default.JsonFilePath = JsonFilePath.Value;
            Properties.Settings.Default.BinFilePath = BinFilePath.Value;
            Properties.Settings.Default.Save();

            //ファイルパスがなければエラーダイアログ表示で終了
            //JSONファイルパスチェック
            if (string.IsNullOrWhiteSpace(JsonFilePath.Value))
            {
                throw new FileNotFoundException(JsonFilePath.Value);
            }

            //BINファイルパスチェック
            if (string.IsNullOrWhiteSpace(BinFilePath.Value))
            {
                throw new FileNotFoundException(BinFilePath.Value);
            }

            ProtocolSetting setting = ProtocolSetting.FromJsonFile(JsonFilePath.Value);
            BinaryParser parser = new BinaryParser(setting);
            ParsedData result = parser.ParseBinaryFile(BinFilePath.Value);

            var formatOption = new TableFormatOption
            {
                UseNumberOption = true,
                ShowIndex = true
            };
            var tableData = new ParsedDataConverter(formatOption).ConvertToTableData(result);
            //TableWindowを表示する
            _windowService.ShowTableWindow(tableData.GetHeaderNames(), tableData.Rows);

        }
    }
}