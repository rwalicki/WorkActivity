using Microsoft.EntityFrameworkCore;
using Work.Core.Models;

namespace Work.Infrastructure.Data
{
    public class AppContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Core.Models.Work> WorkPieces { get; set; }

        public AppContext(DbContextOptions options) : base(options) { }
    }
}