using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class SideBarViewModel : ViewModelBase
    {
        private readonly IMenuService _menuService;

        private readonly NavigationService<SprintListViewModel> _sprintListNavigationService;
        private readonly NavigationService<TaskListViewModel> _taskListNavigationService;
        private readonly NavigationService<WorkListViewModel> _workListNavigationService;
        private readonly NavigationService<DailyWorkListViewModel> _dailyWorkListNavigationService;
        private readonly NavigationService<OffWorkViewModel> _offWorkNavigationService;
        private readonly NavigationService<ReportsViewModel> _reportsNavigationService;

        private readonly NavigationStore _navigationStore;

        private ObservableCollection<MenuItemViewModel> _menuItems;
        public IEnumerable<MenuItemViewModel> MenuItems => _menuItems;

        private MenuItemViewModel _selectedItem;
        public MenuItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand OnLoadCommand { get; }
        public ICommand SelectionChangedCommand { get; }

        public SideBarViewModel(IMenuService menuService,
            NavigationService<SprintListViewModel> sprintListNavigationService,
            NavigationService<TaskListViewModel> taskListNavigationService,
            NavigationService<WorkListViewModel> workListNavigationService,
            NavigationService<DailyWorkListViewModel> dailyWorkListNavigationService,
            NavigationService<OffWorkViewModel> offWorkNavigationService,
            NavigationService<ReportsViewModel> reportsNavigationService,
            NavigationStore navigationStore)
        {
            _menuItems = new ObservableCollection<MenuItemViewModel>();
            _menuService = menuService;

            _sprintListNavigationService = sprintListNavigationService;
            _taskListNavigationService = taskListNavigationService;
            _workListNavigationService = workListNavigationService;
            _dailyWorkListNavigationService = dailyWorkListNavigationService;
            _offWorkNavigationService = offWorkNavigationService;
            _reportsNavigationService = reportsNavigationService;

            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += CurrentViewModelChanged;

            OnLoadCommand = new RelayCommand(Load);
            SelectionChangedCommand = new RelayCommand(SelectionChanged);
        }

        private void CurrentViewModelChanged()
        {
            var name = _navigationStore.CurrentViewModel?.GetType().Name;
            var menuItem = _menuItems.Where(x => x.ContextName.Equals(name)).FirstOrDefault();
            SelectedItem = menuItem;
        }

        private void Load(object obj)
        {
            var menuItems = _menuService.GetMenuItems();
            foreach (var item in menuItems)
            {
                _menuItems.Add(new MenuItemViewModel(item.Item1, item.Item2, item.Item3));
            }
            SelectedItem = _menuItems.Where(x => x.MenuItem.Equals(Enums.MenuItems.Tasks)).First();
        }

        private void SelectionChanged(object args)
        {
            var selectionChangedArgs = args as SelectionChangedEventArgs;
            if (selectionChangedArgs.AddedItems.Count > 0)
            {
                var menuItemViewModel = selectionChangedArgs.AddedItems[0] as MenuItemViewModel;
                Navigate(menuItemViewModel);
            }
        }

        private void Navigate(MenuItemViewModel menuItemViewModel)
        {
            switch (menuItemViewModel.MenuItem)
            {
                case Enums.MenuItems.Sprints:
                    _sprintListNavigationService.Navigate();
                    break;
                case Enums.MenuItems.Tasks:
                    _taskListNavigationService.Navigate();
                    break;
                case Enums.MenuItems.Works:
                    _workListNavigationService.Navigate();
                    break;
                case Enums.MenuItems.DailyWork:
                    _dailyWorkListNavigationService.Navigate();
                    break;
                case Enums.MenuItems.OffWork:
                    _offWorkNavigationService.Navigate();
                    break;
                case Enums.MenuItems.Reports:
                    _reportsNavigationService.Navigate();
                    break;
            }
        }
    }
}