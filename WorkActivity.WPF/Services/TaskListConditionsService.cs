using System;
using Work.Core.Interfaces;
using WorkActivity.WPF.ViewModels;

namespace WorkActivity.WPF.Services
{
    public class TaskListConditionsService : IConditionsService<TaskViewModel>
    {
        private string _searchText;
        private int _sprintId;

        public TaskListConditionsService(string searchText, int sprintId)
        {
            _searchText = searchText;
            _sprintId = sprintId;
        }

        public bool IsSatisfied(TaskViewModel item)
        {
            var textCondition = string.IsNullOrEmpty(_searchText) ||
                item.Date.ToString().Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
                item.Title.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
                item.Name.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
                item.Sprints.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase);

            var sprintCondition = _sprintId.Equals(-1) || item.Task.Sprints.Exists(x => x?.Id == _sprintId);

            return textCondition && sprintCondition;
        }
    }
}