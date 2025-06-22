using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BinaryParserApp.ViewModel
{
    public class TableWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Dictionary<string, string>> Rows { get; } = new();
        public List<string> Columns { get; } = new();

        public TableWindowViewModel(IEnumerable<string> columns, IEnumerable<Dictionary<string, string>> rows)
        {
            Columns.AddRange(columns);
            foreach (var row in rows)
                Rows.Add(row);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}