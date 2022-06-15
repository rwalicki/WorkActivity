using System.Collections.Generic;
using WorkActivity.WPF.Enums;

namespace WorkActivity.WPF.Services
{
    public class MenuService : IMenuService
    {
        public IEnumerable<(MenuItems, string, string)> GetMenuItems()
        {
            var menuItems = new List<(MenuItems, string, string)>();
            menuItems.Add((MenuItems.Sprints, "Sprints", "SprintListViewModel"));
            menuItems.Add((MenuItems.Tasks, "Tasks", "TaskListViewModel"));
            menuItems.Add((MenuItems.Works, "Works", "WorkListViewModel"));
            menuItems.Add((MenuItems.DailyWork, "Daily Work", "DailyWorkListViewModel"));
            menuItems.Add((MenuItems.OffWork, "Off Work", "OffWorkViewModel"));
            menuItems.Add((MenuItems.Reports, "Reports", "ReportsViewModel"));
            return menuItems;
        }
    }
}