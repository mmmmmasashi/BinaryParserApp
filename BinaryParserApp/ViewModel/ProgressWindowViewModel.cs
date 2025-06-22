using Reactive.Bindings;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BinaryParserApp.ViewModel
{
    public class ProgressWindowViewModel
    {
        private readonly Window _window;
        private readonly Task _task;

        public ICommand CancelCommand { get; set; }

        public ProgressWindowViewModel(Window window, Task task, CancellationTokenSource? cts = null)
        {
            _window = window;
            _task = task;

            CancelCommand = new ReactiveCommand().WithSubscribe(() => cts?.Cancel());

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