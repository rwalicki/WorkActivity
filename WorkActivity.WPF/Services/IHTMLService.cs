using System.Collections.Generic;

namespace WorkActivity.WPF.Services
{
    public interface IHTMLService
    {
        void AddTable(IEnumerable<string> header, IEnumerable<IEnumerable<string>> rows);
        string GetContent();
        void Clear();
    }
}