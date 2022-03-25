using Microsoft.EntityFrameworkCore;

namespace Work.Infrastructure.Data
{
    public class AppContextFactory : IDbContextFactory<AppContext>
    {
        private readonly string _connectionString;

        public AppContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AppContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(_connectionString).Options;
            return new AppContext(options);
        }
    }
}