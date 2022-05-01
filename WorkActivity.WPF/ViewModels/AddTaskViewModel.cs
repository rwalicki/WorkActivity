using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using Work.Core.Models;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        private readonly ISnackbarService _snackbarService;
        private readonly ITaskRepository _taskService;
        private readonly ISprintRepository _sprintService;
        private readonly NavigationService<TaskListViewModel> _taskListNavigationService;

        public ObservableCollection<SprintViewModel> Sprints { get; set; }

        public string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
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
            ITaskRepository taskService, 
            ISprintRepository sprintService, 
            NavigationService<TaskListViewModel> taskListNavigationService)
        {
            Sprints = new ObservableCollection<SprintViewModel>();

            _snackbarService = snackbarService;
            _taskService = taskService;
            _sprintService = sprintService;
            _taskListNavigationService = taskListNavigationService;

            OnLoadCommand = new RelayCommand(Load);

            AddTaskCommand = new RelayCommand(async (obj) =>
            {
                var result = await _taskService.Create(new Work.Core.Models.Task()
                {
                    Number = int.Parse(Number),
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
            });
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
    }
}