using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryParserApp.View.Service
{
    public interface IWindowService
    {
        void ShowTextWindow(string text);
        void ShowTableWindow(List<string> headerNames, List<List<string>> rows);
        Task ShowProgressWindow(Task task, CancellationTokenSource? cts = null);
    }
}
