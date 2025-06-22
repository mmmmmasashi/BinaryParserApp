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
            viewModel.RequestClose += () => Close();
            viewModel.RequestCopyToTsv += CopyAsTsv;
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


        private void CopyAsTsv(string tsvContent)
        {
            // クリップボードにTSV形式でコピー
            try
            {
                Clipboard.SetText(tsvContent);
                MessageBox.Show("TSV形式でクリップボードにコピーしました。", "コピー成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TSV形式のコピーに失敗しました。\n{ex.Message}", "コピー失敗", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}