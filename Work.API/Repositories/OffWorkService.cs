using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Work.Core.Interfaces;
using Work.Core.Models;

namespace Work.API.Repositories
{
    public class OffWorkService : IOffWorkService
    {
        private readonly IFileService<OffWork> _offWorkRepository;

        public OffWorkService(IFileService<OffWork> offWorkRepository)
        {
            _offWorkRepository = offWorkRepository;
        }

        public async Task<IEnumerable<DateTime>> GetOffDateList()
        {
            var offDays = await _offWorkRepository.GetAll();
            var dateList = new List<DateTime>();

            foreach (var offDay in offDays)
            {
                var startDate = offDay.StartDate;
                var endDate = offDay.EndDate;

                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    dateList.Add(startDate);
                }

                while (startDate < endDate)
                {
                    startDate = startDate.AddDays(1);
                    if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        dateList.Add(startDate);
                    }
                }
            }

            return dateList;
        }
    }
}