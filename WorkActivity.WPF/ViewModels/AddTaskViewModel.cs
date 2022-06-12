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
    public class AddTaskViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly TaskStore _taskStore;
        private readonly ISprintRepository _sprintRepository;
        private readonly NavigationService<TaskListViewModel> _taskListNavigationService;

        private readonly ObservableCollection<SprintViewModel> _sprints;
        public IEnumerable<SprintViewModel> Sprints => _sprints;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _title;
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
            ISprintRepository sprintRepository,
            NavigationService<TaskListViewModel> taskListNavigationService)
        {
            _sprints = new ObservableCollection<SprintViewModel>();

            _snackbarService = snackbarService;
            _taskStore = taskStore;
            _sprintRepository = sprintRepository;
            _taskListNavigationService = taskListNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            AddTaskCommand = new RelayCommand(AddTask);
        }

        private async void Load(object sender)
        {
            var result = await _sprintRepository.GetAll();
            if (result.Success)
            {
                foreach (var sprint in result.Data)
                {
                    _sprints.Add(new SprintViewModel(sprint));
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