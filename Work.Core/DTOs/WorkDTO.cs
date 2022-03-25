using Shared.Models;
using System;

namespace Work.Core.DTOs
{
    public class WorkDTO : BaseEntity
    {
        public int TaskId { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }
    }
}