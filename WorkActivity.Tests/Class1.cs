using NUnit.Framework;
using System;
using WorkActivity.WPF.Services;

namespace WorkActivity.Tests
{
    [TestFixture]
    public class Class1
    {
        private IHTMLTableRenderer _renderer;

        [SetUp]
        public void SetUp()
        {
            _renderer = new TableRenderer();
        }

        [Test]
        public void TT()
        {
            var header = new string[] { "A", "B" };
            _renderer.AddHeader(header);
            
        }
    }
}
