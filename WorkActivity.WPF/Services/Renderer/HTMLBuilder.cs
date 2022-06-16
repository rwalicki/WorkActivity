using System.IO;
using System.Xml;

namespace WorkActivity.WPF.Services.Renderer
{
    public class HTMLBuilder : IDocumentBuilder
    {
        private const string BODY = "<body>";
        private readonly string _templatePath;
        private readonly string _filePath;

        private string _document;

        public HTMLBuilder(string templatePath)
        {
            _templatePath = templatePath;
            _filePath = _templatePath + Path.DirectorySeparatorChar + "index.html";

            using (var stream = new StreamReader(_filePath))
            {
                _document = stream.ReadToEnd();
            }
        }

        public IDocumentBuilder WithElement(string element)
        {
            var index = _document.IndexOf(BODY) + BODY.Length;
            _document = _document.Insert(index, element);
            return this;
        }

        public string Build()
        {
            return _document;
        }
    }
}