using System;
using System.Xml;

namespace WorkActivity.WPF.Services
{
    public class TableRenderer : IHTMLTableRenderer
    {
        private XmlDocument _doc;

        public TableRenderer()
        {
            _doc = new XmlDocument();
            var element = _doc.CreateElement("table");
            _doc.AppendChild(element);
            var ss = _doc.ToString();
        }

        public void AddHeader(string[] header)
        {
            var node = _doc.FirstChild;
            var head = _doc.CreateElement("thead");
            var row = _doc.CreateElement("tr");
            foreach(var element in header)
            {
                var td = _doc.CreateElement("td");
                td.InnerText = element;
                row.AppendChild(td);
            }
            head.AppendChild(row);
            node.AppendChild(head);
        }

        public void AddRow(string[] row)
        {
            throw new NotImplementedException();
        }

        public string Render()
        {
            return _doc.InnerText;
        }
    }
}
