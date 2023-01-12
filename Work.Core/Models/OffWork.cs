using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Work.Core.Models
{
    [Table("OffWorks")]
    public class OffWork : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}