using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Work.Core.Interfaces
{
    public interface IWorkReportService
    {
        Task<IEnumerable<Models.Work>> Get(DateTime startDate, DateTime endDate);
    }
}