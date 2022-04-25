using MaterialDesignThemes.Wpf;
using Shared.Interfaces;
using System;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, ICloseWindow
    {
        private ITaskRepository _taskService;
        private IWorkRepository _workService;
        private IDailyWorkService _dailyWorkService;
        private ISprintRepository _sprintService;

        private ViewModelBase _currentViewModel;

        public event Action Close;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public SnackbarMessageQueue SnakbarMessageQueue { get; set; }

        public ICommand SprintListCommand { get; set; }
        public ICommand TaskListCommand { get; set; }
        public ICommand WorkListCommand { get; set; }
        public ICommand DailyWorkListCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public MainWindowViewModel(ITaskRepository taskService, IWorkRepository workService, IDailyWorkService dailyWorkService, ISprintRepository sprintService)
        {
            SnakbarMessageQueue = new SnackbarMessageQueue();

            _taskService = taskService;
            _workService = workService;
            _dailyWorkService = dailyWorkService;
            _sprintService = sprintService;

            CurrentViewModel = new TaskListViewModel(_taskService, _workService, _sprintService, this);

            SprintListCommand = new RelayCommand((obj) =>
            {
                CurrentViewModel = new SprintListViewModel(_sprintService, this);
                (CurrentViewModel as SprintListViewModel)?.OnLoadCommand.Execute(null);
            });

            WorkListCommand = new RelayCommand((obj) =>
            {
                CurrentViewModel = new WorkListViewModel(_workService, _taskService, this);
                (CurrentViewModel as WorkListViewModel)?.OnLoadCommand.Execute(null);
            });

            TaskListCommand = new RelayCommand((obj) =>
            {
                CurrentViewModel = new TaskListViewModel(_taskService, _workService, _sprintService, this);
                (CurrentViewModel as TaskListViewModel)?.OnLoadCommand.Execute(null);
            });

            DailyWorkListCommand = new RelayCommand((obj) =>
            {
                CurrentViewModel = new DailyWorkListViewModel(_dailyWorkService, workService, this);
                (CurrentViewModel as DailyWorkListViewModel)?.OnLoadCommand.Execute(null);
            });

            CloseCommand = new RelayCommand((obj) =>
            {
                Close?.Invoke();
            });
        }
    }
}