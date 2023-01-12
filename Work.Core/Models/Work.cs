using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Work.Core.Models
{
    [Table("Works")]
    public class Work : BaseEntity
    {
        public Task Task { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }
    }
}