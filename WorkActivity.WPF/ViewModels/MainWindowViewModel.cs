using MaterialDesignThemes.Wpf;
using Shared.Interfaces;
using System;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IWindowOperations
    {
        private readonly ISnackbarService _snackbarService;
        private readonly NavigationStore _navigationStore;

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
            TopBarViewModel topBarViewModel,
            SideBarViewModel sideBarViewModel)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += CurrentViewModelChanged;

            _modalNavigationStore = modalNavigationStore;
            _modalNavigationStore.CurrentViewModelChanged += CurrentModalViewModelChanged;

            _snackbarService = snackbarService;

            TopBarViewModel = topBarViewModel;
            TopBarViewModel.WindowMinimizeCommand = new RelayCommand((obj) => Minimize?.Invoke());
            TopBarViewModel.WindowMaximizeCommand = new RelayCommand((obj) => Maximize?.Invoke());
            TopBarViewModel.WindowRestoreCommand = new RelayCommand((obj) => Restore?.Invoke());
            TopBarViewModel.CloseCommand = new RelayCommand((obj) => Close?.Invoke());
            WindowMaximized = (isMaximized) => TopBarViewModel.IsMaximized = isMaximized;
            SetWindowTitle = (title) => TopBarViewModel.Title = title;

            SideBarViewModel = sideBarViewModel;
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