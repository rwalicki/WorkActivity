using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Work.Core.Models
{
    [Table("Sprints")]
    public class Sprint : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}