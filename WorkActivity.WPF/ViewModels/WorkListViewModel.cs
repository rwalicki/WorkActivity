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
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly ParameterNavigationService<object, AddWorkViewModel> _addWorkNavigationService;

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
            ModalNavigationStore modalNavigationStore,
            ParameterNavigationService<object, AddWorkViewModel> addWorkNavigationService)
        {
            _snackbarService = snackbarService;
            _workStore = workStore;
            _modalNavigationStore = modalNavigationStore;
            _addWorkNavigationService = addWorkNavigationService;

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
                var submitCommand = new Action<object>(async (task) =>
                {
                    var result = await _workStore.Delete(work.Id);
                    if (result.Success)
                    {
                        _snackbarService.ShowMessage($"Work id {result.Data.Id} removed.");
                        OnLoadCommand.Execute(null);
                    }
                    _modalNavigationStore.CurrentViewModel = null;
                });
                var cancelCommand = new Action(() => _modalNavigationStore.CurrentViewModel = null);
                _modalNavigationStore.CurrentViewModel = new PopupViewModel($"Do you want to remove work from task number: {work.Task.Number}?", obj => submitCommand(work), cancelCommand);
            }
        }
    }
}