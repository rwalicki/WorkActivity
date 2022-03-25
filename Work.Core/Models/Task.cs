using Shared.Models;
using System;

namespace Work.Core.Models
{
    public class Task : BaseEntity
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}