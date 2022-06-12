using MaterialDesignThemes.Wpf;
using Shared.Interfaces;
using System;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Enums;
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

        private readonly ModalNavigationStore _modalNavigationStore;

        public ViewModelBase CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
        public bool IsModalOpen => _modalNavigationStore.IsOpen;

        public TopBarViewModel TopBarViewModel { get; }
        public SideBarViewModel SideBarViewModel { get; }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public SnackbarMessageQueue SnakbarMessageQueue => _snackbarService.GetSnackbar();

        public Action<bool> WindowMaximized { get; set; }
        public Action<string> SetWindowTitle { get; set; }

        public event Action Minimize;
        public event Action Maximize;
        public event Action Restore;
        public event Action Close;

        public MainWindowViewModel(NavigationStore navigationStore,
            ModalNavigationStore modalNavigationStore,
            ISnackbarService snackbarService,
            NavigationService<SprintListViewModel> sprintListNavigationService,
            NavigationService<TaskListViewModel> taskListNavigationService,
            NavigationService<WorkListViewModel> workListNavigationService,
            NavigationService<DailyWorkListViewModel> dailyWorkListNavigationService,
            NavigationService<OffWorkViewModel> offWorkNavigationService,
            NavigationService<ReportsViewModel> reportsNavigationService,
            TopBarViewModel topBarViewModel,
            SideBarViewModel sideBarViewModel)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += CurrentViewModelChanged;

            _modalNavigationStore = modalNavigationStore;
            _modalNavigationStore.CurrentViewModelChanged += CurrentModalViewModelChanged;

            _snackbarService = snackbarService;

            _sprintListNavigationService = sprintListNavigationService;
            _taskListNavigationService = taskListNavigationService;
            _workListNavigationService = workListNavigationService;
            _dailyWorkListNavigationService = dailyWorkListNavigationService;
            _offWorkNavigationService = offWorkNavigationService;
            _reportsNavigationService = reportsNavigationService;

            TopBarViewModel = topBarViewModel;
            TopBarViewModel.WindowMinimizeCommand = new RelayCommand((obj) => Minimize?.Invoke());
            TopBarViewModel.WindowMaximizeCommand = new RelayCommand((obj) => Maximize?.Invoke());
            TopBarViewModel.WindowRestoreCommand = new RelayCommand((obj) => Restore?.Invoke());
            TopBarViewModel.CloseCommand = new RelayCommand((obj) => Close?.Invoke());
            WindowMaximized = (isMaximized) => TopBarViewModel.IsMaximized = isMaximized;
            SetWindowTitle = (title) => TopBarViewModel.Title = title;

            SideBarViewModel = sideBarViewModel;
            SideBarViewModel.SelectionChangedCommand = new RelayCommand(Navigate);
        }

        private void CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void CurrentModalViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpen));
        }

        public override void Dispose()
        {
            _modalNavigationStore.CurrentViewModelChanged -= CurrentModalViewModelChanged;
            _navigationStore.CurrentViewModelChanged -= CurrentViewModelChanged;
        }

        private void Navigate(object obj)
        {
            var parameter = obj as MenuItemViewModel;
            switch (parameter.MenuItem)
            {
                case MenuItems.Sprints:
                    _sprintListNavigationService.Navigate();
                    break;
                case MenuItems.Tasks:
                    _taskListNavigationService.Navigate();
                    break;
                case MenuItems.Works:
                    _workListNavigationService.Navigate();
                    break;
                case MenuItems.DailyWork:
                    _dailyWorkListNavigationService.Navigate();
                    break;
                case MenuItems.OffWork:
                    _offWorkNavigationService.Navigate();
                    break;
                case MenuItems.Reports:
                    _reportsNavigationService.Navigate();
                    break;
            }
        }
    }
}