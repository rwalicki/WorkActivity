using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class SideBarViewModel : ViewModelBase
    {
        private readonly IMenuService _menuService;

        private ObservableCollection<MenuItemViewModel> _menuItems;
        public IEnumerable<MenuItemViewModel> MenuItems => _menuItems;

        private MenuItemViewModel _selectedItem;
        public MenuItemViewModel SelectedItem
        {
            get => _selectedItem; set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand SelectionChangedCommand { get; set; }
        public ICommand OnLoadCommand { get; set; }
        public SideBarViewModel(IMenuService menuService)
        {
            _menuItems = new ObservableCollection<MenuItemViewModel>();
            _menuService = menuService;
            OnLoadCommand = new RelayCommand(Load);
        }

        private void Load(object obj)
        {
            var menuItems = _menuService.GetMenuItems();
            foreach (var item in menuItems)
            {
                _menuItems.Add(new MenuItemViewModel(item.Item1, item.Item2));
            }
            SelectedItem = _menuItems.Where(x => x.MenuItem.Equals(Enums.MenuItems.Tasks)).First();
        }
    }
}