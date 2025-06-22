using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Reactive.Bindings;

namespace BinaryParserApp.ViewModel
{
    public class TableWindowViewModel : INotifyPropertyChanged
    {
        public List<string> Columns { get; } = new();
        public ObservableCollection<List<string>> Rows { get; } = new();

        public ReactiveCommand CloseCommand { get; }
        public ReactiveCommand CopyAsTsvCommand { get; }
        public event Action? RequestClose;
        public event Action<string>? RequestCopyToTsv;


        public TableWindowViewModel(IEnumerable<string> columns, IEnumerable<List<string>> rows)
        {
            Columns.AddRange(columns);
            foreach (var row in rows)
                Rows.Add(row);

            CloseCommand = new ReactiveCommand().WithSubscribe(() => RequestClose?.Invoke());
            CopyAsTsvCommand = new ReactiveCommand().WithSubscribe(() => RequestCopyToTsv?.Invoke(CreateTsv()));
        }

        private string CreateTsv()
        {
            var tsvBuilder = new System.Text.StringBuilder();
            // ヘッダー行
            tsvBuilder.AppendLine(string.Join("\t", Columns));
            // データ行
            foreach (var row in Rows)
            {
                tsvBuilder.AppendLine(string.Join("\t", row));
            }
            return tsvBuilder.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}