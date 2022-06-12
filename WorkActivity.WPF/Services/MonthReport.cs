using System;
using System.Linq;
using System.Threading.Tasks;
using Work.Core.Interfaces;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.Services
{
    public class MonthReport : IReport
    {
        private readonly DailyWorkStore _dailyWorkStore;
        private readonly IOffWorkService _offWorkService;

        public MonthReport(DailyWorkStore dailyWorkStore, IOffWorkService offWorkService)
        {   
            _dailyWorkStore = dailyWorkStore;
            _offWorkService = offWorkService;
        }

        public decimal GetExpectedHours(int month, int year)
        {
            var today = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            if (DateTime.Today.Year.Equals(year) && DateTime.Today.Month.Equals(month))
            {
                today = DateTime.Today;
            }

            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var task = Task.Run(() => { return _offWorkService.GetOffDateList(); });
            task.Wait();
            var offWorkDays = task.Result;
            var days = 0;
            for (int i = 1; i <= today.Day; i++)
            {
                var date = new DateTime(today.Year, today.Month, i);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !offWorkDays.Contains(date.Date))
                {
                    days++;
                }
            }

            return days * 7.5m;
        }

        public decimal GetLoggedHours(int month, int year)
        {
            var task = Task.Run(async () => 
            {
                await _dailyWorkStore.Load();
                return _dailyWorkStore.DailyWorks;
            });

            task.Wait();
            var dailyWorks = task.Result;

            var loggedDailyWorks = dailyWorks.Where(x => x.Date.Month == month && x.Date.Year == year).ToList();
            return (decimal)loggedDailyWorks.Select(x => x.Hours).Sum() * 1.0m;
        }

        public decimal GetMissingHours(int month, int year)
        {
            var expectedHours = GetExpectedHours(month, year);
            var loggedHours = GetLoggedHours(month, year);
            return expectedHours - loggedHours;
        }
    }
}