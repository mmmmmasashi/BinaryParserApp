using System.Threading.Tasks;
using System.Windows;

namespace BinaryParserApp.View.Service
{
    public class WindowService : IWindowService
    {
        public void ShowTextWindow(string text)
        {
            var window = new View.TextWindow(text);
            window.Owner = GetActiveWindow();
            window.ShowDialog();
        }

        public void ShowTableWindow(List<string> headerNames, List<List<string>> rows)
        {
            var viewModel = new ViewModel.TableWindowViewModel(headerNames, rows);
            var window = new TableWindow(viewModel);
            window.Owner = GetActiveWindow();
            window.ShowDialog();
        }

        public async Task ShowProgressWindow(Task task, CancellationTokenSource? cts = null)
        {
            var window = new ProgressWindow();
            var viewModel = new ViewModel.ProgressWindowViewModel(window, task, cts);
            window.DataContext = viewModel;
            window.Owner = GetActiveWindow();
            window.ShowDialog();
            await task;
        }

        private static Window? GetActiveWindow()
        {
            return Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
        }
    }
}
