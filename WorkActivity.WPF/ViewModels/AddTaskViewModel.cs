using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using Work.Core.Models;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        private ITaskRepository _taskService;
        private IWorkRepository _workService;
        private ISprintRepository _sprintService;
        private MainWindowViewModel _mainWindowViewModel;

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

        public ICommand AddTaskCommand { get; set; }
        public ICommand SelectSprintCommand { get; set; }

        public AddTaskViewModel(ITaskRepository taskService, IWorkRepository workService, ISprintRepository sprintService, MainWindowViewModel mainWindowViewModel, List<Sprint> sprints)
        {
            Sprints = new ObservableCollection<SprintViewModel>();
            foreach (var sprint in sprints)
            {
                Sprints.Add(new SprintViewModel(sprint));
            }

            _sprintService = sprintService;
            _taskService = taskService;
            _workService = workService;
            _mainWindowViewModel = mainWindowViewModel;
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
                    _mainWindowViewModel.CurrentViewModel = new TaskListViewModel(_taskService, _workService, _sprintService, _mainWindowViewModel);
                }
                else
                {
                    _mainWindowViewModel.SnakbarMessageQueue.Enqueue(result.Message);
                }
            });
        }
    }
}