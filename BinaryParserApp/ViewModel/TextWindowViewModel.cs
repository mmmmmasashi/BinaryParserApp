using System;
using System.Windows.Input;
using BinaryParserApp.View.Service;
using Reactive.Bindings;

namespace BinaryParserApp.ViewModel
{
    public class TextWindowViewModel
    {
        public ReactiveProperty<string> DisplayText { get; set; }
        public ICommand CloseCommand { get; }

        public event Action? RequestClose;

        public TextWindowViewModel(string text)
        {
            DisplayText = new ReactiveProperty<string>(text);
            CloseCommand = new ReactiveCommand()
                .WithSubscribe(() => RequestClose?.Invoke());
        }
    }
}