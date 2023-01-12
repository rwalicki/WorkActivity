using Microsoft.EntityFrameworkCore;
using Work.Core.Interfaces;

namespace Work.Infrastructure.Data
{
    public class WorkActivityContextFactory : IDbContextFactory<WorkActivityContext>
    {
        private readonly IConfigurationService _configurationService;

        public WorkActivityContextFactory(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public WorkActivityContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder().UseSqlite(_configurationService.GetConnectionString()).Options;
            return new WorkActivityContext(options);
        }
    }
}