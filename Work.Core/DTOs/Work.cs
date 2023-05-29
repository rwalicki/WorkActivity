using Shared.Models;
using System;

namespace Work.Core.DTOs
{
    public class Work : BaseEntity
    {
        public int TaskId { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }
        public bool IsOverWork { get; set; }
    }
}