using MaterialDesignThemes.Wpf;
using Shared.Interfaces;
using System;
using System.Windows.Input;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, ICloseWindow
    {
        private readonly ISnackbarService _snackbarService;
        private readonly NavigationStore _navigationStore;

        private readonly NavigationService<SprintListViewModel> _sprintListNavigationService;
        private readonly NavigationService<TaskListViewModel> _taskListNavigationService;
        private readonly NavigationService<WorkListViewModel> _workListNavigationService;
        private readonly NavigationService<DailyWorkListViewModel> _dailyWorkListNavigationService;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public SnackbarMessageQueue SnakbarMessageQueue => _snackbarService.GetSnackbar();

        public event Action Close;

        public ICommand SprintListCommand { get; set; }
        public ICommand TaskListCommand { get; set; }
        public ICommand WorkListCommand { get; set; }
        public ICommand DailyWorkListCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public MainWindowViewModel(NavigationStore navigationStore,
            ISnackbarService snackbarService,
            NavigationService<SprintListViewModel> sprintListNavigationService,
            NavigationService<TaskListViewModel> taskListNavigationService,
            NavigationService<WorkListViewModel> workListNavigationService,
            NavigationService<DailyWorkListViewModel> dailyWorkListNavigationService)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));

            _snackbarService = snackbarService;

            _sprintListNavigationService = sprintListNavigationService;
            _taskListNavigationService = taskListNavigationService;
            _workListNavigationService = workListNavigationService;
            _dailyWorkListNavigationService = dailyWorkListNavigationService;

            SprintListCommand = new NavigateCommand(_sprintListNavigationService);
            TaskListCommand = new NavigateCommand(_taskListNavigationService);
            WorkListCommand = new NavigateCommand(_workListNavigationService);
            DailyWorkListCommand = new NavigateCommand(_dailyWorkListNavigationService);

            CloseCommand = new RelayCommand((obj) => Close?.Invoke());
        }
    }
}