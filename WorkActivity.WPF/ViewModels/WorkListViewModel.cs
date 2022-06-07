using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class WorkListViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly WorkStore _workStore;
        private readonly ParameterNavigationService<object, AddWorkViewModel> _addWorkNavigationService;
        private readonly DailyProgressStore _dailyProgressStore;
        private readonly IDailyWorkService _dailyWorkService;

        private List<Work.Core.Models.Work> _works;

        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ItemView.Refresh();
            }
        }

        public ICollectionView ItemView { get; set; }

        public ICommand OnLoadCommand { get; set; }
        public ICommand AddWorkCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public WorkListViewModel(ISnackbarService snackbarService,
            WorkStore workStore,
            ParameterNavigationService<object, AddWorkViewModel> addWorkNavigationService, 
            DailyProgressStore dailyProgressStore,
            IDailyWorkService dailyWorkService)
        {
            _snackbarService = snackbarService;
            _workStore = workStore;
            _addWorkNavigationService = addWorkNavigationService;
            _dailyProgressStore = dailyProgressStore;
            _dailyWorkService = dailyWorkService;

            OnLoadCommand = new RelayCommand(Load);
            AddWorkCommand = new RelayCommand(AddWorkNavigate);
            DeleteCommand = new RelayCommand(Delete);
        }

        private bool Filter(object sender)
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return true;
            }

            var work = sender as Work.Core.Models.Work;
            if (work != null)
            {
                return work.Date.ToString().Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                    work.Task.Title.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                    work.Task.Number.ToString().Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private async void Load(object obj)
        {
            await _workStore.Load();
            _works = _workStore.Works.OrderByDescending(x => x.Date).ToList();
            ItemView = CollectionViewSource.GetDefaultView(_works);
            ItemView.Filter = Filter;
            ItemView.Refresh();
            OnPropertyChanged(nameof(ItemView));

            var dailyWorks = (await _dailyWorkService.GetAll()).ToList();
            var element = dailyWorks.FirstOrDefault();
            
            if (element != null && element.Date.Date.Equals(DateTime.Today.Date))
            {
                _dailyProgressStore.Hours = element?.Hours ?? 0;
            }
            else
            {
                _dailyProgressStore.Hours = 0;
            }
        }

        private void AddWorkNavigate(object sender)
        {
            _addWorkNavigationService.Navigate(null);
        }

        private async void Delete(object sender)
        {
            var work = sender as Work.Core.Models.Work;
            if (work != null)
            {
                var result = await _workStore.Delete(work.Id);
                if (result.Success)
                {
                    _snackbarService.ShowMessage($"Work id {result.Data.Id} removed.");
                    OnLoadCommand.Execute(null);
                }
            }
        }
    }
}