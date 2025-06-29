using BinaryParserApp.View.Service;
using BinaryParserLib.Common;
using BinaryParserLib.Parsed;
using BinaryParserLib.Parser;
using BinaryParserLib.Protocol;
using BinaryParserLib.Setting;
using BinaryParserLib.Text;
using Reactive.Bindings;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        public string TitleText { get; private set; }

        // JSON設定ファイルパス
        public ReactiveProperty<string> JsonFilePath { get; set; }

        // 設定ファイルの候補一覧
        public ObservableCollection<string> SettingFileCandidates { get; private set; }

        // BINファイルパス
        public ReactiveProperty<string> BinFilePath { get; set; }

        // 変換コマンド
        public ReactiveCommand ConvertCommand { get; }

        private SettingsFileHistory _settingsFileHistory;

        public MainWindowViewModel(IWindowService windowService)
        {
            var history = Properties.Settings.Default.JsonFilePathHistory;
            _settingsFileHistory = new SettingsFileHistory(history);
            SettingFileCandidates = new ObservableCollection<string>(_settingsFileHistory.GetHistory());

            _windowService = windowService;
            TitleText = CreateAppTitle();
            // 設定から前回値を復元
            JsonFilePath = new ReactiveProperty<string>(Properties.Settings.Default.JsonFilePath);
            BinFilePath = new ReactiveProperty<string>(Properties.Settings.Default.BinFilePath);

            

            // 両方のパスが入力されているときだけボタンを有効化
            var canConvert = JsonFilePath
                .CombineLatest(BinFilePath, (json, bin) => !string.IsNullOrWhiteSpace(json) && !string.IsNullOrWhiteSpace(bin));

            ConvertCommand = canConvert.ToReactiveCommand();

            ConvertCommand.Subscribe(async _ => await ConvertAsync());
        }

        /// <summary>
        /// タイトル文字列の作成 例)「BinaryParserApp ver1.0.1.0」
        /// </summary>
        private string CreateAppTitle()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version == null)
            {
                return "BinaryParserApp";
            }
            return $"BinaryParserApp ver{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async Task ConvertAsync()
        {
            try
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

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;

                var settingFilePath = PathUtil.RemoveDoubleQuatation(JsonFilePath.Value);

                //設定ファイル履歴を更新
                {
                    _settingsFileHistory.Add(settingFilePath);
                    SettingFileCandidates.Clear();
                    foreach (var item in _settingsFileHistory.GetHistory())
                    {
                        SettingFileCandidates.Add(item);
                    }
                    JsonFilePath.Value = settingFilePath;

                    // 履歴を保存
                    var storage = new StringCollection();
                    _settingsFileHistory.SaveToStorage(storage);
                    Properties.Settings.Default.JsonFilePathHistory = storage;
                    Properties.Settings.Default.Save();
                }

                // 非同期でパース処理を実行
                var task = Task.Run<TableData>(() =>
                {
                    ProtocolSetting setting = ProtocolSetting.FromJsonFile(settingFilePath);
                    BinaryParser parser = new BinaryParser(setting, token);

                    ParsedData result = (File.Exists(BinFilePath.Value)) ? 
                        parser.ParseBinaryFile(BinFilePath.Value) : parser.ParseBinaryString(BinFilePath.Value);

                    var formatOption = new TableFormatOption
                    {
                        UseNumberOption = true,
                        UseIndex = true,
                        UseByteSize = true,
                        UseParsedValue = true,
                    };
                    return new ParsedDataConverter(formatOption).ConvertToTableData(result);
                }, token);

                // プログレスウィンドウを表示
                await _windowService.ShowProgressWindow(task, cancellationTokenSource);
                var tableData = task.Result;
                //TableWindowを表示する
                _windowService.ShowTableWindow(tableData.GetHeaderNames(), tableData.Rows);
            }
            catch (OperationCanceledException)
            {
                //ユーザー操作によるキャンセルなので優しく終わる
                MessageBox.Show("処理を中断しました", "中断", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}