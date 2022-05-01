using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class WorkListViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly IWorkRepository _workRepository;
        private readonly NavigationService<AddWorkViewModel> _addWorkNavigationService;

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
            IWorkRepository workRepository,
            NavigationService<AddWorkViewModel> addWorkNavigationService)
        {
            _snackbarService = snackbarService;
            _workRepository = workRepository;
            _addWorkNavigationService = addWorkNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            AddWorkCommand = new RelayCommand(AddWorkNavigate);
            DeleteCommand = new RelayCommand(Delete);
        }

        private async void Delete(object sender)
        {
            var work = sender as Work.Core.Models.Work;
            if (work != null)
            {
                var result = await _workRepository.Delete(work.Id);
                if (result.Success)
                {
                    _snackbarService.ShowMessage($"Work id {result.Data.Id} removed.");
                    OnLoadCommand.Execute(null);
                }
            }
        }

        private void AddWorkNavigate(object sender)
        {
            _addWorkNavigationService.Navigate();
        }

        private async void Load(object obj)
        {
            var result = await _workRepository.GetAll();
            if (result.Success)
            {
                _works = result.Data.OrderByDescending(x => x.Date).ToList();
                ItemView = CollectionViewSource.GetDefaultView(_works);
                ItemView.Filter = Filter;
                ItemView.Refresh();
                OnPropertyChanged(nameof(ItemView));
            }
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
    }
}