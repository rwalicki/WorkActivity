using System.Collections.Generic;
using WorkActivity.WPF.Enums;

namespace WorkActivity.WPF.Services
{
    public class MenuService : IMenuService
    {
        public IEnumerable<(MenuItems, string)> GetMenuItems()
        {
            var menuItems = new List<(MenuItems, string)>();
            menuItems.Add((MenuItems.Sprints, "Sprints"));
            menuItems.Add((MenuItems.Tasks, "Tasks"));
            menuItems.Add((MenuItems.Works, "Works"));
            menuItems.Add((MenuItems.DailyWork, "Daily Work"));
            menuItems.Add((MenuItems.OffWork, "Off Work"));
            menuItems.Add((MenuItems.Reports, "Reports"));
            return menuItems;
        }
    }
}