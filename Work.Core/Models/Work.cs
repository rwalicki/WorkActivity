using Shared.Models;
using System;

namespace Work.Core.Models
{
    public class Work : BaseEntity
    {
        public Task Task { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }
    }
}