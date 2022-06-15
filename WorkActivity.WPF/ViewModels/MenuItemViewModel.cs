using WorkActivity.WPF.Enums;

namespace WorkActivity.WPF.ViewModels
{
    public class MenuItemViewModel : ViewModelBase
    {
        private MenuItems _menuItem;
        public MenuItems MenuItem => _menuItem;

        private string _name;
        public string Name => _name;

        private string _contextName;
        public string ContextName => _contextName;

        public MenuItemViewModel(MenuItems menuItem, string name, string contextName)
        {
            _menuItem = menuItem;
            _name = name;
            _contextName = contextName;
        }
    }
}