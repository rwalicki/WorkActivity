using System;
using System.Collections.Generic;

namespace Work.Core.Models
{
    public class DailyWork
    {
        public List<int> WorkIds { get; set; }
        public DateTime Date { get; set; }
        public float Hours { get; set; }

        public DailyWork()
        {
            WorkIds = new List<int>();
        }
    }
}