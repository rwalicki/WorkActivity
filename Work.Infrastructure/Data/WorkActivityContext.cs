using Microsoft.EntityFrameworkCore;
using Work.Core.Models;

namespace Work.Infrastructure.Data
{
    public class WorkActivityContext : DbContext
    {
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Core.Models.Work> Works { get; set; }
        public DbSet<OffWork> OffWorks { get; set; }

        public WorkActivityContext(DbContextOptions options) : base(options)
        {

        }
    }
}