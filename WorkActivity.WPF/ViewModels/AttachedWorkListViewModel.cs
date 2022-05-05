using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class AttachedWorkListViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly IWorkRepository _workRepository;
        private readonly ParameterNavigationService<object, AddWorkViewModel> _addWorkNavigationService;
        private readonly TaskViewModel _task;

        public string TaskTitle => $"{_task.Number}: {_task.Title}";

        public ICollectionView ItemView { get; set; }

        public ICommand OnLoadCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public AttachedWorkListViewModel(ISnackbarService snackbarService,
            IWorkRepository workRepository,
            ParameterNavigationService<object, AddWorkViewModel> addWorkNavigationService,
            object task)
        {
            _snackbarService = snackbarService;
            _workRepository = workRepository;
            _addWorkNavigationService = addWorkNavigationService;
            _task = task as TaskViewModel;

            OnLoadCommand = new RelayCommand(Load);
            DeleteCommand = new RelayCommand(Delete);
        }

        private async void Load(object obj)
        {
            var result = await _workRepository.GetAll();
            if (result.Success)
            {
                var works = result.Data.Where(x => x.Task.Id == _task.Id).OrderByDescending(x => x.Date).ToList();
                ItemView = CollectionViewSource.GetDefaultView(works);
                ItemView.Refresh();
                OnPropertyChanged(nameof(ItemView));
            }
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
    }
}