using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Work.Core.Interfaces;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.Services
{
    public class WorkReportService : IWorkReportService
    {
        private readonly WorkStore _workStore;

        public WorkReportService(WorkStore workStore)
        {
            _workStore = workStore;
        }

        public async Task<IEnumerable<Work.Core.Models.Work>> Get(DateTime startDate, DateTime endDate)
        {
            await _workStore.Load();
            var works = _workStore.Works;
            var filteredWorks = works.Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date).OrderBy(x=>x.Date);
            return filteredWorks;
        }
    }
}