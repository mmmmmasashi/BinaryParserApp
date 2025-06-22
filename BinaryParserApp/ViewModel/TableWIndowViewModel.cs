using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BinaryParserApp.ViewModel
{
    public class TableWindowViewModel : INotifyPropertyChanged
    {
        public List<string> Columns { get; } = new();
        public ObservableCollection<List<string>> Rows { get; } = new();

        public TableWindowViewModel(IEnumerable<string> columns, IEnumerable<List<string>> rows)
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