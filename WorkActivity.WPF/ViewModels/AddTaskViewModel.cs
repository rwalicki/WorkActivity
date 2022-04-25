using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using System.Collections.Generic;
using Work.Core.Models;
using System.Linq;

namespace WorkActivity.WPF.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        private ITaskRepository _taskService;
        private IWorkRepository _workService;
        private ISprintRepository _sprintService;
        private MainWindowViewModel _mainWindowViewModel;

        public List<Work.Core.Models.Sprint> Sprints { get; set; }

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

        public HashSet<int> SelectedSprints { get; set; }

        public ICommand AddTaskCommand { get; set; }
        public ICommand SelectSprintCommand { get; set; }


        public AddTaskViewModel(ITaskRepository taskService, IWorkRepository workService, ISprintRepository sprintService, MainWindowViewModel mainWindowViewModel, List<Sprint> sprints)
        {
            SelectedSprints = new HashSet<int>();
            Sprints = sprints;
            _sprintService = sprintService;
            _taskService = taskService;
            _workService = workService;
            _mainWindowViewModel = mainWindowViewModel;
            AddTaskCommand = new RelayCommand(async(obj) =>
            {
                var result = await _taskService.Create(new Work.Core.Models.Task() 
                { 
                    Number = int.Parse(Number), 
                    Title = Title, 
                    Date = System.DateTime.Now,
                    Sprints = Sprints.Where(x=> SelectedSprints.Contains(x.Id)).ToList()
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