using System.Collections.Generic;
using Work.Core.Interfaces;
using WorkActivity.WPF.Services.Renderer;

namespace WorkActivity.WPF.Services
{
    public class HTMLService : IHTMLService
    {
        private readonly IConfigurationService _configurationService;

        private string _content;

        public HTMLService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void AddTable(IEnumerable<string> header, IEnumerable<IEnumerable<string>> rows)
        {
            var builder = new HTMLTableBuilder().WithHeader(header);
            foreach (var row in rows)
            {
                builder = builder.WithRow(row);
            }
            _content += builder.Build();
        }

        public string GetContent()
        {
            return new HTMLBuilder(_configurationService.GetPDFTemplatePath())
                .WithElement(_content)
                .Build();
        }

        public void Clear()
        {
            _content = string.Empty;
        }
    }
}