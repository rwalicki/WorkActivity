using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class TaskListViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;

        private readonly TaskStore _taskStore;

        private readonly ISprintRepository _sprintRepository;
        private readonly WorkStore _workStore;
        private readonly IFilterService<TaskViewModel> _filterService;
        private readonly TaskListViewStore _taskListViewStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly NavigationService<AddTaskViewModel> _addTaskNavigationService;
        private readonly ParameterNavigationService<object, AddWorkViewModel> _addWorkNavigationService;
        private readonly ParameterNavigationService<object, EditTaskViewModel> _editTaskNavigationService;
        private readonly ParameterNavigationService<object, AttachedWorkListViewModel> _attachedWorkListNavigationService;

        public ObservableCollection<SprintViewModel> Sprints { get; set; }
        private SprintViewModel _selectedSprint;
        public SprintViewModel SelectedSprint
        {
            get => _selectedSprint;
            set
            {
                _selectedSprint = value;
                _taskListViewStore.SelectedSprintId = value?.Sprint.Id ?? -1;
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
        public ICommand EditTaskCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddWorkCommand { get; set; }
        public ICommand ShowWorksCommand { get; set; }

        public TaskListViewModel(ISnackbarService snackbarService,
            TaskStore taskStore,
            ISprintRepository sprintRepository,
            WorkStore workStore,
            IFilterService<TaskViewModel> filterService,
            TaskListViewStore taskListViewStore,
            NavigationService<AddTaskViewModel> addTaskNavigationService,
            ParameterNavigationService<object, AddWorkViewModel> addWorkNavigationService,
            ParameterNavigationService<object, EditTaskViewModel> editTaskNavigationService,
            ParameterNavigationService<object, AttachedWorkListViewModel> attachedWorkListNavigationService, 
            ModalNavigationStore modalNavigationStore)
        {
            _snackbarService = snackbarService;
            _taskStore = taskStore;
            _sprintRepository = sprintRepository;
            _workStore = workStore;
            _filterService = filterService;
            _taskListViewStore = taskListViewStore;
            _addTaskNavigationService = addTaskNavigationService;
            _editTaskNavigationService = editTaskNavigationService;
            _addWorkNavigationService = addWorkNavigationService;
            _attachedWorkListNavigationService = attachedWorkListNavigationService;
            _modalNavigationStore = modalNavigationStore;

            OnLoadCommand = new RelayCommand(Load);
            AddTaskCommand = new RelayCommand(AddTaskNavigate);
            EditTaskCommand = new RelayCommand(EditTaskNavigate);
            DeleteCommand = new RelayCommand(async (obj) => await Delete(obj));
            AddWorkCommand = new RelayCommand(AddWork);
            ShowWorksCommand = new RelayCommand(ShowWorks);

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
            SelectActiveSprint();
            await LoadTasks();
        }

        private async Task LoadSprints()
        {
            var sprintResult = await _sprintRepository.GetAll();
            if (sprintResult.Success)
            {
                var minDate = DateTime.Now;
                var maxDate = DateTime.Now;
                if (sprintResult.Data.Any())
                {
                    minDate = sprintResult.Data.Min(x => x.StartDate);
                    maxDate = sprintResult.Data.Max(x => x.EndDate);
                }

                var allOption = new SprintViewModel(new Work.Core.Models.Sprint() { Id = -1, Name = "All", StartDate = minDate, EndDate = maxDate });
                Sprints.Add(allOption);
                var sprints = sprintResult.Data.OrderByDescending(x => x.StartDate);
                foreach (var sprint in sprints)
                {
                    Sprints.Add(new SprintViewModel(sprint));
                }
            }
        }

        private async Task LoadTasks()
        {
            await _taskStore.Load();

            var tasks = _taskStore.Tasks.OrderByDescending(x => x.Date).ToList().Select(x => new TaskViewModel(x));
            ItemView = CollectionViewSource.GetDefaultView(tasks);
            ItemView.Filter = Filter;
            ItemView.Refresh();
            OnPropertyChanged(nameof(ItemView));
        }

        private void AddTaskNavigate(object sender)
        {
            _addTaskNavigationService.Navigate();
        }

        private void EditTaskNavigate(object sender)
        {
            _editTaskNavigationService.Navigate(sender);
        }

        private async Task Delete(object sender)
        {
            var task = sender as TaskViewModel;
            if (task != null)
            {
                await _workStore.Load();
                if (_workStore.Works.Any(x => x.Task.Id.Equals(task.Id)))
                {
                    _snackbarService.ShowMessage($"Cannot remove task {task.Name}. It has works attached.");
                    return;
                }

                var submitCommand = new Action<object>(async (task) =>
                {
                    var result = await _taskStore.Delete((task as TaskViewModel).Id);
                    if (result.Success)
                    {
                        _snackbarService.ShowMessage($"Task {(task as TaskViewModel).Name} removed.");
                        await LoadTasks();
                    }
                    _modalNavigationStore.CurrentViewModel = null;
                });

                var cancelCommand = new Action(() => _modalNavigationStore.CurrentViewModel = null);

                _modalNavigationStore.CurrentViewModel = new PopupViewModel($"Do you want to remove task {task.Name}?",obj => submitCommand(task), cancelCommand);
            }
        }

        private void AddWork(object sender)
        {
            _addWorkNavigationService.Navigate(sender);
        }

        private void ShowWorks(object sender)
        {
            _attachedWorkListNavigationService.Navigate(sender);
        }

        private void SelectActiveSprint()
        {
            var activeSprint = Sprints.FirstOrDefault(x => x.Sprint.Id == _taskListViewStore.SelectedSprintId);
            if (activeSprint != null)
            {
                SelectedSprint = activeSprint;
            }
            else
            {
                SelectedSprint = Sprints.First();
            }
        }
    }
}