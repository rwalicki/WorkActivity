using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Work.Core.Interfaces
{
    public interface IOffWorkService
    {
        Task<IEnumerable<DateTime>> GetOffDateList();
    }
}