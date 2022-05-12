using Shared.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Work.Core.Interfaces;

namespace WorkActivity.WPF.Services
{
    public class MonthReport : IReport
    {
        private int _currentMonthWorkDays;
        private readonly IDailyWorkService _dailyWorkService;
        private readonly IFileService<Work.Core.Models.OffWork> _offWorkService;

        public MonthReport(IDailyWorkService dailyWorkService, IFileService<Work.Core.Models.OffWork> offWorkService)
        {
            _currentMonthWorkDays = GetCurrentMonthWorkDays();
            _dailyWorkService = dailyWorkService;
            _offWorkService = offWorkService;
        }

        public decimal GetExpectedHours()
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var offWorkDays = _offWorkService.GetAll();
            var days = 0;
            for (int i = 1; i <= today.Day; i++)
            {
                var date = new DateTime(today.Year, today.Month, i);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    days++;
                }
            }

            return days * 7.5m;
        }

        public decimal GetLoggedHours()
        {
            var task = Task.Run(() => { return _dailyWorkService.GetAll(); });
            task.Wait();
            var dailyWorks = task.Result;
            
            var loggedDailyWorks = dailyWorks.Where(x => x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year).ToList();
            return (decimal)loggedDailyWorks.Select(x => x.Hours).Sum();
        }

        public decimal GetMissingHours()
        {
            var expectedHours = GetExpectedHours();
            var loggedHours = GetLoggedHours();
            return expectedHours - loggedHours;
        }

        private int GetCurrentMonthWorkDays()
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).Day;

            var days = 0;
            for (int i = 1; i <= lastDayOfMonth; i++)
            {
                var date = new DateTime(today.Year, today.Month, i);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    days++;
                }
            }
            return days;
        }
    }
}