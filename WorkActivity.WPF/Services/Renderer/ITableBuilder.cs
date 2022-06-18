using System.Collections.Generic;

namespace WorkActivity.WPF.Services.Renderer
{
    public interface ITableBuilder
    {
        ITableBuilder WithHeader(IEnumerable<string> header);
        ITableBuilder WithRow(IEnumerable<string> row);
        string Build();
    }
}