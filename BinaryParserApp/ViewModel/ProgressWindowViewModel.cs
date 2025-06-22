using System;
using System.Threading.Tasks;
using System.Windows;

namespace BinaryParserApp.ViewModel
{
    public class ProgressWindowViewModel
    {
        private readonly Window _window;
        private readonly Task _task;

        public ProgressWindowViewModel(Window window, Task task)
        {
            _window = window;
            _task = task;

            // タスクが完了したらウィンドウを閉じる
            _task.ContinueWith(_ => 
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _window.Close();
                });
            });
        }
    }
}