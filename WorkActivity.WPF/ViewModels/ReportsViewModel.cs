using System.Windows.Input;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly IReport _reportService;

        private decimal _expectedHours;
        public decimal ExpectedHours
        {
            get => _expectedHours;
            set
            {
                _expectedHours = value;
                OnPropertyChanged(nameof(ExpectedHours));
            }
        }

        private decimal _loggedHours;
        public decimal LoggedHours
        {
            get => _loggedHours;
            set
            {
                _loggedHours = value;
                OnPropertyChanged(nameof(LoggedHours));
            }
        }

        private decimal _missingHours;
        public decimal MissingHours
        {
            get => _missingHours;
            set
            {
                _missingHours = value;
                OnPropertyChanged(nameof(MissingHours));
            }
        }

        public ICommand OnLoadCommand { get; }

        public ReportsViewModel(IReport reportService)
        {
            _reportService = reportService;

            OnLoadCommand = new RelayCommand(s => Load(s));
        }

        private void Load(object sender)
        {
            ExpectedHours = _reportService.GetExpectedHours();
            LoggedHours = _reportService.GetLoggedHours();
            MissingHours = _reportService.GetMissingHours();
        }
    }
}