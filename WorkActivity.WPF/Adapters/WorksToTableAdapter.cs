using System.Collections.Generic;
using System.Linq;

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

            var mergedWorks = works
                .GroupBy(w => w.Task.Name)
                .Select(g => new Work.Core.Models.Work
                {
                    Task = new Work.Core.Models.Task
                    {
                        Name = g.Key,
                        Title = g.First().Task.Title
                    },
                    Date = g.First().Date,
                    Hours = g.Sum(w => w.Hours)
                })
                .ToList();

            var i = 1;
            var rows = new List<List<string>>();
            foreach (var work in mergedWorks)
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