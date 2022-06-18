using System.Collections.Generic;

namespace WorkActivity.WPF.Adapters
{
    public interface IWorksToTableAdapter
    {
        (IEnumerable<string>, IEnumerable<IEnumerable<string>>) GetTableData(IEnumerable<Work.Core.Models.Work> works);
    }
}