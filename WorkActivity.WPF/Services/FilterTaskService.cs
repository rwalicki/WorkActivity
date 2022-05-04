using Work.Core.Interfaces;
using WorkActivity.WPF.ViewModels;

namespace WorkActivity.WPF.Services
{
    public class FilterTaskService : IFilterService<TaskViewModel>
    {
        public bool Filter(TaskViewModel item, IConditionsService<TaskViewModel> conditionsService)
        {
            return conditionsService.IsSatisfied(item);
        }
    }
}