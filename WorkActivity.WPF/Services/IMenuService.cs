using System.Collections.Generic;
using WorkActivity.WPF.Enums;

namespace WorkActivity.WPF.Services
{
    public interface IMenuService
    {
        IEnumerable<(MenuItems, string, string)> GetMenuItems();
    }
}