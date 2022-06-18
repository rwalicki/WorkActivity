using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Work.Core.Interfaces;
using WorkActivity.WPF.Adapters;

namespace WorkActivity.WPF.Services
{
    public class PdfGeneratorFacade : IPdfGeneratorFacade
    {
        private readonly IConfigurationService _configurationService;
        private readonly IWorksToTableAdapter _worksToTableAdapter;
        private readonly IHTMLService _htmlService;
        private readonly IPdfService _pdfService;

        public PdfGeneratorFacade(IConfigurationService configurationService, IWorksToTableAdapter worksToTableAdapter, IHTMLService htmlService, IPdfService pdfService)
        {
            _configurationService = configurationService;
            _worksToTableAdapter = worksToTableAdapter;
            _htmlService = htmlService;
            _pdfService = pdfService;
        }

        public async Task GeneratePdf(IEnumerable<Work.Core.Models.Work> works)
        {
            var (header, rows) = _worksToTableAdapter.GetTableData(works);

            _htmlService.Clear();
            _htmlService.AddTable(header, rows);
            var content = _htmlService.GetContent();

            var dlg = new SaveFileDialog();
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.Filter = "PDF|*.pdf";

            var result = dlg.ShowDialog() ?? false;
            if (result)
            {
                await _pdfService.GeneratePdf(dlg.FileName, content);
            }
        }
    }
}