using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class EditTaskViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly ITaskRepository _taskService;
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

        public ObservableCollection<SprintViewModel> Sprints { get; set; }

        public string Number
        {
            get { return _task.Number.ToString(); }
            set
            {
                _task.Number = int.Parse(value);
                OnPropertyChanged(nameof(Number));
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
            ITaskRepository taskService,
            ISprintRepository sprintService,
            NavigationService<TaskListViewModel> taskListNavigationService,
            object task)
        {
            Sprints = new ObservableCollection<SprintViewModel>();
            Task = (task as TaskViewModel)?.Task;

            _snackbarService = snackbarService;
            _taskService = taskService;
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
                    Sprints.Add(new SprintViewModel(sprint));
                }
            }

            var ids = _task.Sprints.Select(x => x.Id);
            foreach (var sprint in Sprints)
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

            var result = await _taskService.Update(_task);
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