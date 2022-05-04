using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ISprintRepository _sprintRepository;
        private readonly IFilterService<TaskViewModel> _filterService;
        private readonly NavigationService<AddTaskViewModel> _addTaskNavigationService;
        private readonly ParameterNavigationService<object, AddWorkViewModel> _addWorkNavigationService;

        public ObservableCollection<SprintViewModel> Sprints { get; set; }
        private SprintViewModel _selectedSprint;
        public SprintViewModel SelectedSprint
        {
            get => _selectedSprint;
            set
            {
                _selectedSprint = value;
                OnPropertyChanged(nameof(SelectedSprint));
                ItemView?.Refresh();
            }
        }

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


        public TaskListViewModel(ISnackbarService snackbarService,
            ITaskRepository taskRepository,
            ISprintRepository sprintRepository,
            IFilterService<TaskViewModel> filterService,
            NavigationService<AddTaskViewModel> addTaskNavigationService,
            ParameterNavigationService<object, AddWorkViewModel> addWorkNavigationService)
        {
            _snackbarService = snackbarService;
            _taskRepository = taskRepository;
            _sprintRepository = sprintRepository;
            _filterService = filterService;
            _addTaskNavigationService = addTaskNavigationService;
            _addWorkNavigationService = addWorkNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            AddTaskCommand = new RelayCommand(AddTaskNavigate);
            DeleteCommand = new RelayCommand(async (obj) => await Delete(obj));
            OnAddWorkItem = new RelayCommand(AddWorkItem);

            Sprints = new ObservableCollection<SprintViewModel>();
        }

        private bool Filter(object sender)
        {
            var conditionsService = new TaskListConditionsService(SearchText, SelectedSprint?.Sprint.Id ?? -1);
            return _filterService.Filter(sender as TaskViewModel, conditionsService);
        }

        private async void Load(object sender)
        {
            await LoadSprints();
            await LoadTasks(sender);
        }

        private async Task LoadSprints()
        {
            var sprintResult = await _sprintRepository.GetAll();
            if (sprintResult.Success)
            {
                Sprints.Clear();
                var minDate = DateTime.Now;
                var maxDate = DateTime.Now;
                if (sprintResult.Data.Any())
                {
                    minDate = sprintResult.Data.Min(x => x.StartDate);
                    maxDate = sprintResult.Data.Max(x => x.EndDate);
                }

                SelectedSprint = new SprintViewModel(new Work.Core.Models.Sprint() { Id = -1, Name = "All", StartDate = minDate, EndDate = maxDate });
                Sprints.Add(SelectedSprint);

                foreach (var sprint in sprintResult.Data)
                {
                    Sprints.Add(new SprintViewModel(sprint));
                }
            }
        }

        private async Task LoadTasks(object sender)
        {
            var result = await _taskRepository.GetAll();
            if (result.Success)
            {
                var tasks = result.Data.OrderByDescending(x => x.Date).ToList().Select(x => new TaskViewModel(x));
                ItemView = CollectionViewSource.GetDefaultView(tasks);
                ItemView.Filter = Filter;
                ItemView.Refresh();
                OnPropertyChanged(nameof(ItemView));
            }
        }

        private void AddTaskNavigate(object sender)
        {
            _addTaskNavigationService.Navigate();
        }

        private async Task Delete(object sender)
        {
            var task = sender as TaskViewModel;
            if (task != null)
            {
                var result = await _taskRepository.Delete(task.Id);
                if (result.Success)
                {
                    _snackbarService.ShowMessage($"Task number {result.Data.Number} removed.");
                    await LoadTasks(sender);
                }
            }
        }

        private void AddWorkItem(object sender)
        {
            _addWorkNavigationService.Navigate(sender);
        }
    }
}