using Shared.Interfaces;
using System;
using System.Windows.Input;
using Work.Core.Models;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class AddOffWorkViewModel : ViewModelBase
    {
        private IFileService<OffWork> _offWorkRepository;
        private NavigationService<OffWorkViewModel> _navigationService;

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public ICommand AddOffWorkCommand { get; }

        public AddOffWorkViewModel(IFileService<OffWork> offWorkRepository, NavigationService<OffWorkViewModel> navigationService)
        {
            _offWorkRepository = offWorkRepository;
            _navigationService = navigationService;

            AddOffWorkCommand = new RelayCommand(AddOffWork);
        }

        private async void AddOffWork(object sender)
        {
            var result = await _offWorkRepository.Create(new OffWork() { StartDate = StartDate, EndDate = EndDate });
            _navigationService.Navigate();
        }
    }
}