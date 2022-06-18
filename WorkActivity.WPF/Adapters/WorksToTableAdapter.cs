using System.Collections.Generic;

namespace WorkActivity.WPF.Adapters
{
    public class WorksToTableAdapter : IWorksToTableAdapter
    {
        public (IEnumerable<string>, IEnumerable<IEnumerable<string>>) GetTableData(IEnumerable<Work.Core.Models.Work> works)
        {
            var header = new List<string>()
            {
                "No.", "Name", "Title", "Date", "Hours"
            };

            var i = 1;
            var rows = new List<List<string>>();
            foreach (var work in works)
            {
                rows.Add(new List<string>()
                {
                    $"{i++}",
                    work.Task.Name,
                    work.Task.Title,
                    work.Date.ToString("dd.MM.yyyy"),
                    work.Hours.ToString()
                });
            }

            return (header, rows);
        }
    }
}