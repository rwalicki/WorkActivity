using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly TaskStore _taskStore;
        private readonly ISprintRepository _sprintService;
        private readonly NavigationService<TaskListViewModel> _taskListNavigationService;

        public ObservableCollection<SprintViewModel> Sprints { get; set; }

        public string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public ICommand OnLoadCommand { get; set; }
        public ICommand AddTaskCommand { get; set; }

        public AddTaskViewModel(ISnackbarService snackbarService,
            TaskStore taskStore,
            ISprintRepository sprintService,
            NavigationService<TaskListViewModel> taskListNavigationService)
        {
            Sprints = new ObservableCollection<SprintViewModel>();

            _snackbarService = snackbarService;
            _taskStore = taskStore;
            _sprintService = sprintService;
            _taskListNavigationService = taskListNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            AddTaskCommand = new RelayCommand(AddTask);
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
        }

        private async void AddTask(object sender)
        {
            var result = await _taskStore.Create(new Work.Core.Models.Task()
            {
                Name = Name,
                Title = Title,
                Date = System.DateTime.Now,
                Sprints = Sprints.Where(x => x.IsSelected).Select(x => x.Sprint).ToList()
            });

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