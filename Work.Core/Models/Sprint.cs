using Shared.Models;
using System;

namespace Work.Core.Models
{
    public class Sprint : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}