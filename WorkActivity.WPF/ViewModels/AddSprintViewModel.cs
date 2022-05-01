using System;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class AddSprintViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly ISprintRepository _sprintRepository;
        private readonly NavigationService<SprintListViewModel> _sprintListNavigationService;

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

        public AddSprintViewModel(ISnackbarService snackbarService,
            ISprintRepository sprintRepository,
            NavigationService<SprintListViewModel> sprintListNavigationService)
        {
            _snackbarService = snackbarService;
            _sprintRepository = sprintRepository;
            _sprintListNavigationService = sprintListNavigationService;

            AddSprintCommand = new RelayCommand(AddSprint);
        }

        private async void AddSprint(object sender)
        {
            var result = await _sprintRepository.Create(new Work.Core.Models.Sprint() { Name = Name, StartDate = StartDate, EndDate = EndDate });
            if (result.Success)
            {
                _sprintListNavigationService.Navigate();
            }
            else
            {
                _snackbarService.ShowMessage(result.Message);
            }
        }
    }
}