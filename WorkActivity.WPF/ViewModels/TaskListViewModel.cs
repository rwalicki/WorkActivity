using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class TaskListViewModel : ViewModelBase
    {
        private readonly ITaskRepository _taskService;
        private readonly IWorkRepository _workService;
        private readonly ISprintService _sprintService;
        private readonly MainWindowViewModel _mainWindowViewModel;

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
        public ICommand OnDeleteCommand { get; set; }
        public ICommand OnAddWorkItem { get; set; }


        public TaskListViewModel(ITaskRepository taskService, IWorkRepository workService, ISprintService sprintService, MainWindowViewModel mainWindowViewModel)
        {
            _taskService = taskService;
            _workService = workService;
            _sprintService = sprintService;
            _mainWindowViewModel = mainWindowViewModel;

            OnLoadCommand = new RelayCommand(async (obj) =>
            {
                var result = await _taskService.GetAll();
                if (result.Success)
                {
                    _tasks = result.Data.OrderByDescending(x => x.Date).ToList();
                    ItemView = CollectionViewSource.GetDefaultView(_tasks);
                    ItemView.Filter = Filter;
                    ItemView.Refresh();
                    OnPropertyChanged(nameof(ItemView));
                }
            });

            AddTaskCommand = new RelayCommand(async (obj) =>
            {
                var sprints = (await _sprintService.GetAll()).Data.ToList();
                _mainWindowViewModel.CurrentViewModel = new AddTaskViewModel(_taskService, _workService, _sprintService, _mainWindowViewModel, sprints);
            });

            OnDeleteCommand = new RelayCommand(async (obj) =>
            {
                var task = obj as Work.Core.Models.Task;
                if (task != null)
                {
                    var result = await _taskService.Delete(task.Id);
                    if (result.Success)
                    {
                        _mainWindowViewModel.SnakbarMessageQueue.Enqueue($"Task number {result.Data.Number} removed.");
                        OnLoadCommand.Execute(null);
                    }
                }
            });

            OnAddWorkItem = new RelayCommand(async (obj) =>
            {
                var tasks = (await _taskService.GetAll()).Data.OrderByDescending(x => x.Date).ToList();
                _mainWindowViewModel.CurrentViewModel = new AddWorkViewModel(_workService, _taskService, _mainWindowViewModel, tasks, obj as Work.Core.Models.Task);
            });
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
    }
}