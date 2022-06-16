using NUnit.Framework;
using System;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Services.Renderer;

namespace WorkActivity.Tests
{
    [TestFixture]
    public class Class1
    {
        private HTMLTableBuilder _renderer;

        [SetUp]
        public void SetUp()
        {
            _renderer = new HTMLTableBuilder();
        }

        [Test]
        public void TT()
        {
            var header = new string[] { "A", "B" };
            var row1 = new string[] { "A1", "B1" };
            var row2 = new string[] { "A2", "B2" };
            var row3 = new string[] { "A3", "B3" };
            var s = _renderer
                .WithHeader(header)
                .WithRow(row1)
                .WithRow(row2)
                .WithRow(row3)
                .Build();
            
        }
    }
}
