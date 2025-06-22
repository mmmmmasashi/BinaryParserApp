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

        private static Window? GetActiveWindow()
        {
            return Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
        }
    }
}
