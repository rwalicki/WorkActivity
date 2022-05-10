using Shared.Models;
using System;

namespace Work.Core.Models
{
    public class OffWork : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}