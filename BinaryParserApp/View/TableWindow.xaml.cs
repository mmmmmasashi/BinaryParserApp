using System.Windows;
using System.Windows.Controls;
using BinaryParserApp.ViewModel;

namespace BinaryParserApp.View
{
    public partial class TableWindow : Window
    {
        public TableWindow(TableWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // カラムをColumnsの順に動的生成
            for (int i = 0; i < viewModel.Columns.Count; i++)
            {
                var colIndex = i; // クロージャ対策
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = viewModel.Columns[i],
                    Binding = new System.Windows.Data.Binding($"[{colIndex}]")
                });
            }
        }
    }
}