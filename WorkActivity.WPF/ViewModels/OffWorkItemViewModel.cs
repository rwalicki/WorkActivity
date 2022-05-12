using System;
using Work.Core.Models;

namespace WorkActivity.WPF.ViewModels
{
    public class OffWorkItemViewModel : ViewModelBase
    {
        private OffWork _offWork;
        public OffWork OffWork => _offWork;

        public string StartDate => _offWork.StartDate.ToString("dd.MM.yyyy");

        public string EndDate => _offWork.EndDate.ToString("dd.MM.yyyy");

        public int DaysOff { get; private set; }

        public OffWorkItemViewModel(OffWork offWork)
        {
            _offWork = offWork;
            CalculateDays();
        }

        private void CalculateDays()
        {
            var startDate = _offWork.StartDate;
            var endDate = _offWork.EndDate;
            var days = 0;

            if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
            {
                days++;
            }

            while (startDate < endDate)
            {
                startDate = startDate.AddDays(1);
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    days++;
                }
            }
            
            DaysOff = days;
        }
    }
}