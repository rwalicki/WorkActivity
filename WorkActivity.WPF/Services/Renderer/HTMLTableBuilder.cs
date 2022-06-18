using System.Collections.Generic;
using System.Xml;

namespace WorkActivity.WPF.Services.Renderer
{
    public class HTMLTableBuilder : ITableBuilder
    {
        private XmlDocument _doc;
        private XmlElement _table;
        private XmlElement _tbody;

        private const string TABLE = "table";
        private const string THEAD = "thead";
        private const string TBODY = "tbody";
        private const string HEAD = "th";
        private const string ROW = "tr";
        private const string DATA = "td";


        public HTMLTableBuilder()
        {
            _doc = new XmlDocument();
            _table = CreateElement(TABLE);
            AppendChild(_doc, _table);

            _tbody = CreateElement(TBODY);
            AppendChild(_table, _tbody);
        }

        public ITableBuilder WithHeader(IEnumerable<string> header)
        {
            var thead = CreateElement(THEAD);
            AppendChild(_table, thead);

            var rowElement = CreateElement(ROW);
            AppendChild(thead, rowElement);

            foreach (var head in header)
            {
                var element = CreateElement(HEAD, head);
                AppendChild(rowElement, element);
            }
            return this;
        }

        public ITableBuilder WithRow(IEnumerable<string> row)
        {
            var rowElement = CreateElement(ROW);
            AppendChild(_tbody, rowElement);

            foreach (var value in row)
            {
                var element = CreateElement(DATA, value);
                AppendChild(rowElement, element);
            }

            return this;
        }

        public string Build()
        {
            return _doc.InnerXml.ToString();
        }

        private XmlElement CreateElement(string name)
        {
            return _doc.CreateElement(name);
        }

        private XmlElement CreateElement(string name, string innerText)
        {
            var element = _doc.CreateElement(name);
            element.InnerText = innerText;
            return element;
        }

        private void AppendChild(XmlNode parent, XmlElement child)
        {
            parent.AppendChild(child);
        }
    }
}