using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Work.Core.Interfaces;
using Work.Core.Models;

namespace Work.API.Repositories
{
    public class DailyWorkService : IDailyWorkService
    {
        private readonly ITaskRepository _taskService;
        private readonly IWorkRepository _workService;

        public DailyWorkService(ITaskRepository taskService, IWorkRepository workService)
        {
            _taskService = taskService;
            _workService = workService;
        }

        public async Task<IEnumerable<DailyWork>> GetAll()
        {
            var dailyWorks = new List<DailyWork>();
            var works = await _workService.GetAll();
            if (works.Success)
            {
                foreach (var work in works.Data)
                {
                    if (dailyWorks.Exists(x => x.Date == work.Date.Date))
                    {
                        var dailyWork = dailyWorks.First(x => x.Date == work.Date.Date);
                        dailyWork.Hours += work.Hours;
                        dailyWork.WorkIds.Add(work.Id);
                    }
                    else
                    {
                        var dailyWork = new DailyWork() { Date = work.Date.Date, Hours = work.Hours };
                        dailyWork.WorkIds.Add(work.Id);
                        dailyWorks.Add(dailyWork);
                    }
                }
            }
            return dailyWorks.OrderByDescending(x=>x.Date);
        }
    }
}