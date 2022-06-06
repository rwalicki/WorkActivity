using System.Windows.Input;

namespace WorkActivity.WPF.ViewModels
{
    public class TopBarViewModel : ViewModelBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private bool _isMaximized;
        public bool IsMaximized
        {
            get { return _isMaximized; }
            set
            {
                _isMaximized = value;
                OnPropertyChanged(nameof(IsMaximized));
            }
        }

        public DailyProgressViewModel DailyProgressViewModel { get; }

        public ICommand WindowMinimizeCommand { get; set; }
        public ICommand WindowRestoreCommand { get; set; }
        public ICommand WindowMaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public TopBarViewModel(DailyProgressViewModel dailyProgressViewModel)
        {
            DailyProgressViewModel = dailyProgressViewModel;
        }
    }
}