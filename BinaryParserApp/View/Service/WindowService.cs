using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BinaryParserApp.View.Service
{
    public class WindowService : IWindowService
    {
        public void ShowTextWindow(string text)
        {
            var window = new View.TextWindow(text);
            window.Owner = System.Windows.Application.Current.Windows
                        .OfType<Window>().FirstOrDefault(w => w.IsActive);
            window.ShowDialog();
        }
    }
}
