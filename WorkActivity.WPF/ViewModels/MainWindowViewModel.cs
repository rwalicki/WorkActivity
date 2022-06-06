using MaterialDesignThemes.Wpf;
using Shared.Interfaces;
using System;
using System.Windows.Input;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IWindowOperations
    {
        private readonly ISnackbarService _snackbarService;
        private readonly NavigationStore _navigationStore;

        private readonly NavigationService<SprintListViewModel> _sprintListNavigationService;
        private readonly NavigationService<TaskListViewModel> _taskListNavigationService;
        private readonly NavigationService<WorkListViewModel> _workListNavigationService;
        private readonly NavigationService<DailyWorkListViewModel> _dailyWorkListNavigationService;
        private readonly NavigationService<OffWorkViewModel> _offWorkNavigationService;
        private readonly NavigationService<ReportsViewModel> _reportsNavigationService;

        private bool _isMaximized;
        public bool IsMaximized
        {
            get => _isMaximized;
            set
            {
                _isMaximized = value;
                OnPropertyChanged(nameof(IsMaximized));
            }
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public SnackbarMessageQueue SnakbarMessageQueue => _snackbarService.GetSnackbar();

        public DailyProgressViewModel DailyProgressViewModel { get; private set; }

        public Action<bool> WindowMaximized { get; set; }
        
        public event Action Minimize;
        public event Action Maximize;
        public event Action Restore;
        public event Action Close;

        public ICommand SprintListCommand { get; set; }
        public ICommand TaskListCommand { get; set; }
        public ICommand WorkListCommand { get; set; }
        public ICommand DailyWorkListCommand { get; set; }
        public ICommand OffWorkCommand { get; set; }
        public ICommand ReportsCommand { get; set; }

        public ICommand WindowMinimizeCommand { get; set; }
        public ICommand WindowMaximizeCommand { get; set; }
        public ICommand WindowRestoreCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public MainWindowViewModel(NavigationStore navigationStore,
            ISnackbarService snackbarService,
            NavigationService<SprintListViewModel> sprintListNavigationService,
            NavigationService<TaskListViewModel> taskListNavigationService,
            NavigationService<WorkListViewModel> workListNavigationService,
            NavigationService<DailyWorkListViewModel> dailyWorkListNavigationService,
            NavigationService<OffWorkViewModel> offWorkNavigationService,
            NavigationService<ReportsViewModel> reportsNavigationService,
            DailyProgressViewModel dailyProgressViewModel)
        {
            DailyProgressViewModel = dailyProgressViewModel;
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));

            _snackbarService = snackbarService;

            WindowMaximized = (isMaximized) => IsMaximized = isMaximized;

            _sprintListNavigationService = sprintListNavigationService;
            _taskListNavigationService = taskListNavigationService;
            _workListNavigationService = workListNavigationService;
            _dailyWorkListNavigationService = dailyWorkListNavigationService;
            _offWorkNavigationService = offWorkNavigationService;
            _reportsNavigationService = reportsNavigationService;

            SprintListCommand = new NavigateCommand(_sprintListNavigationService);
            TaskListCommand = new NavigateCommand(_taskListNavigationService);
            WorkListCommand = new NavigateCommand(_workListNavigationService);
            DailyWorkListCommand = new NavigateCommand(_dailyWorkListNavigationService);
            OffWorkCommand = new NavigateCommand(_offWorkNavigationService);
            ReportsCommand = new NavigateCommand(_reportsNavigationService);

            WindowMinimizeCommand = new RelayCommand((obj) => Minimize?.Invoke());
            WindowMaximizeCommand = new RelayCommand((obj) => Maximize?.Invoke());
            WindowRestoreCommand = new RelayCommand((obj) => Restore?.Invoke());
            CloseCommand = new RelayCommand((obj) => Close?.Invoke());
        }
    }
}