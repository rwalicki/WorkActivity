using WorkActivity.WPF.Enums;

namespace WorkActivity.WPF.ViewModels
{
    public class MenuItemViewModel : ViewModelBase
    {
        private MenuItems _menuItem;
        public MenuItems MenuItem => _menuItem;

        private string _name;
        public string Name => _name;

        public MenuItemViewModel(MenuItems menuItem, string name)
        {
            _menuItem = menuItem;
            _name = name;
        }
    }
}