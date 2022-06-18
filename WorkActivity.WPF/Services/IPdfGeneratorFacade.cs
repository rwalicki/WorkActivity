using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkActivity.WPF.Services
{
    public interface IPdfGeneratorFacade
    {
        Task GeneratePdf(IEnumerable<Work.Core.Models.Work> works);
    }
}