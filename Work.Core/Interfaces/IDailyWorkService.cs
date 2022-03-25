using System.Collections.Generic;
using System.Threading.Tasks;
using Work.Core.Models;

namespace Work.Core.Interfaces
{
    public interface IDailyWorkService
    {
        Task<IEnumerable<DailyWork>> GetAll();
    }
}