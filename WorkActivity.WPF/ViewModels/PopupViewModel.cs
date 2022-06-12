using System;
using System.Windows.Input;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class PopupViewModel : ViewModelBase
    {
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public PopupViewModel(string message, Action<object> submitCommand, Action cancelCommand)
        {
            Message = message;
            SubmitCommand = new RelayCommand(submitCommand);
            CancelCommand = new RelayCommand((obj) => cancelCommand());
        }
    }
}