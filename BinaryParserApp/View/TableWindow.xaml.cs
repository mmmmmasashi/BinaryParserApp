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

            // カラムを動的に生成, vm.Columnsに列名が格納されている & Rowsに同名のキーのデータが格納
            //例)
            //var columns = new List<string> { "ID", "Name", "Value" };
            //var rows = new List<Dictionary<string, object>>
            //{
            //    new Dictionary<string, object> { { "ID", 1 }, { "Name", "Item1" }, { "Value", 100 } },
            //    new Dictionary<string, object> { { "ID", 2 }, { "Name", "Item2" }, { "Value", 200 } }
            //};
            foreach (var col in viewModel.Columns)
            {
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = col,
                    Binding = new System.Windows.Data.Binding($"[{col}]")
                });
            }
        }
    }
}