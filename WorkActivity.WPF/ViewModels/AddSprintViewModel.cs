using System;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class AddSprintViewModel : ViewModelBase
    {
        private ISprintService _sprintService;
        private MainWindowViewModel _mainWindowViewModel;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                EndDate = _startDate + TimeSpan.FromDays(13);
            }
        }

        private DateTime _endDate = DateTime.Now + TimeSpan.FromDays(13);
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public ICommand AddSprintCommand { get; set; }

        public AddSprintViewModel(ISprintService sprintService, MainWindowViewModel mainWindowViewModel)
        {
            _sprintService = sprintService;
            _mainWindowViewModel = mainWindowViewModel;
            AddSprintCommand = new RelayCommand(async (obj) =>
            {
                var result = await _sprintService.Create(new Work.Core.Models.Sprint() { Name = Name, StartDate = StartDate, EndDate = EndDate });
                if (result.Success)
                {
                    _mainWindowViewModel.CurrentViewModel = new SprintListViewModel(_sprintService, _mainWindowViewModel);
                }
                else
                {
                    _mainWindowViewModel.SnakbarMessageQueue.Enqueue(result.Message);
                }
            });
        }
    }
}