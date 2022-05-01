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
    public class TaskListViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly ITaskRepository _taskRepository;
        private readonly NavigationService<AddTaskViewModel> _addTaskNavigationService;


        private List<Work.Core.Models.Task> _tasks;
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
        public ICommand AddTaskCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OnAddWorkItem { get; set; }


        public TaskListViewModel(ISnackbarService snackbarService, ITaskRepository taskRepository, NavigationService<AddTaskViewModel> addTaskNavigationService)
        {
            _taskRepository = taskRepository;
            _snackbarService = snackbarService;
            _addTaskNavigationService = addTaskNavigationService;

            OnLoadCommand = new RelayCommand(Load);

            AddTaskCommand = new RelayCommand(AddTaskNavigate);

            DeleteCommand = new RelayCommand(Delete);

            //TODO
            //OnAddWorkItem = new RelayCommand(async (obj) =>
            //{
            //    var tasks = (await _taskService.GetAll()).Data.OrderByDescending(x => x.Date).ToList();
            //    _navigationStore.CurrentViewModel = new AddWorkViewModel(_workService, _taskService, _mainWindowViewModel, tasks, obj as Work.Core.Models.Task, _navigationStore);
            //});
        }

        private bool Filter(object sender)
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return true;
            }

            var task = sender as Work.Core.Models.Task;
            if (task != null)
            {
                return task.Date.ToString().Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                    task.Title.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                    task.Number.ToString().Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private async void Load(object sender)
        {
            var result = await _taskRepository.GetAll();
            if (result.Success)
            {
                _tasks = result.Data.OrderByDescending(x => x.Date).ToList();
                ItemView = CollectionViewSource.GetDefaultView(_tasks);
                ItemView.Filter = Filter;
                ItemView.Refresh();
                OnPropertyChanged(nameof(ItemView));
            }
        }

        private void AddTaskNavigate(object sender)
        {
            _addTaskNavigationService.Navigate();
        }

        private async void Delete(object sender)
        {
            var task = sender as Work.Core.Models.Task;
            if (task != null)
            {
                var result = await _taskRepository.Delete(task.Id);
                if (result.Success)
                {
                    _snackbarService.ShowMessage($"Task number {result.Data.Number} removed.");
                    Load(sender);
                }
            }
        }
    }
}