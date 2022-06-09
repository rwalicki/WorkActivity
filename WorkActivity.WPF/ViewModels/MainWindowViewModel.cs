﻿using MaterialDesignThemes.Wpf;
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

        private readonly ModalNavigationStore _modalNavigationStore;

        public ViewModelBase CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
        public bool IsModalOpen => _modalNavigationStore.IsOpen;

        public TopBarViewModel TopBarViewModel { get; }
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public SnackbarMessageQueue SnakbarMessageQueue => _snackbarService.GetSnackbar();

        public Action<bool> WindowMaximized { get; set; }
        public Action<string> SetWindowTitle { get; set; }
        
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

        public MainWindowViewModel(NavigationStore navigationStore,
            ModalNavigationStore modalNavigationStore,
            ISnackbarService snackbarService,
            NavigationService<SprintListViewModel> sprintListNavigationService,
            NavigationService<TaskListViewModel> taskListNavigationService,
            NavigationService<WorkListViewModel> workListNavigationService,
            NavigationService<DailyWorkListViewModel> dailyWorkListNavigationService,
            NavigationService<OffWorkViewModel> offWorkNavigationService,
            NavigationService<ReportsViewModel> reportsNavigationService,
            TopBarViewModel topBarViewModel)
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

            SprintListCommand = new NavigateCommand(_sprintListNavigationService);
            TaskListCommand = new NavigateCommand(_taskListNavigationService);
            WorkListCommand = new NavigateCommand(_workListNavigationService);
            DailyWorkListCommand = new NavigateCommand(_dailyWorkListNavigationService);
            OffWorkCommand = new NavigateCommand(_offWorkNavigationService);
            ReportsCommand = new NavigateCommand(_reportsNavigationService);

            TopBarViewModel = topBarViewModel;
            TopBarViewModel.WindowMinimizeCommand = new RelayCommand((obj) => Minimize?.Invoke());
            TopBarViewModel.WindowMaximizeCommand = new RelayCommand((obj) => Maximize?.Invoke());
            TopBarViewModel.WindowRestoreCommand = new RelayCommand((obj) => Restore?.Invoke());
            TopBarViewModel.CloseCommand = new RelayCommand((obj) => Close?.Invoke());
            WindowMaximized = (isMaximized) => TopBarViewModel.IsMaximized = isMaximized;
            SetWindowTitle = (title) => TopBarViewModel.Title = title;
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
    }
}