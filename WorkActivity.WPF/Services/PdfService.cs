using System.IO;
using System.Threading.Tasks;
using WkHtmlToPdfDotNet;

namespace WorkActivity.WPF.Services
{
    public class PdfService : IPdfService
    {
        private readonly SynchronizedConverter _converter;

        public PdfService()
        {
            _converter = new SynchronizedConverter(new PdfTools());
        }

        public async Task GeneratePdf(string path, string content)
        {
            await Task.Run(() =>
            {
                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings =
                    {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                    },
                    Objects =
                    {
                        new ObjectSettings()
                        {
                            HtmlContent = content,
                            WebSettings = { DefaultEncoding = "utf-8" }
                        }
                    }
                };

                var pdf = _converter.Convert(doc);
                using (var stream = new FileStream(path,FileMode.Create))
                {
                    stream.Write(pdf, 0, pdf.Length);
                }
            });
        }
    }
}