using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class EditTaskViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly TaskStore _taskStore;
        private readonly ISprintRepository _sprintService;
        private readonly NavigationService<TaskListViewModel> _taskListNavigationService;

        private Work.Core.Models.Task _task;
        public Work.Core.Models.Task Task
        {
            get { return _task; }
            set
            {
                _task = value;
                OnPropertyChanged(nameof(Task));
            }
        }

        private readonly ObservableCollection<SprintViewModel> _sprints;
        public IEnumerable<SprintViewModel> Sprints => _sprints;

        public string Name
        {
            get { return _task.Name.ToString(); }
            set
            {
                _task.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Title
        {
            get { return _task.Title; }
            set
            {
                _task.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public ICommand OnLoadCommand { get; set; }
        public ICommand EditTaskCommand { get; set; }

        public EditTaskViewModel(ISnackbarService snackbarService,
            TaskStore taskStore,
            ISprintRepository sprintService,
            NavigationService<TaskListViewModel> taskListNavigationService,
            object task)
        {
            _sprints = new ObservableCollection<SprintViewModel>();
            Task = (task as TaskViewModel)?.Task;

            _snackbarService = snackbarService;
            _taskStore = taskStore;
            _sprintService = sprintService;
            _taskListNavigationService = taskListNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            EditTaskCommand = new RelayCommand(EditTask);
        }

        private async void Load(object sender)
        {
            var result = await _sprintService.GetAll();
            if (result.Success)
            {
                foreach (var sprint in result.Data)
                {
                    _sprints.Add(new SprintViewModel(sprint));
                }
            }

            var ids = _task.Sprints.Select(x => x.Id);
            foreach (var sprint in _sprints)
            {
                if (ids.Contains(sprint.Sprint.Id))
                {
                    sprint.IsSelected = true;
                }
            }
        }

        private async void EditTask(object sender)
        {
            _task.Sprints = Sprints.Where(x => x.IsSelected).Select(x => x.Sprint).ToList();

            var result = await _taskStore.Update(_task);
            if (result.Success)
            {
                _taskListNavigationService.Navigate();
            }
            else
            {
                _snackbarService.ShowMessage(result.Message);
            }
        }
    }
}