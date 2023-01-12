using Microsoft.Extensions.Configuration;
using Work.Core.Interfaces;

namespace WorkActivity.WPF.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetDatabasePath()
        {
            var path = _configuration.GetSection("Database")?["path"]?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + "data";
            }
            return path;
        }

        public string GetConnectionString()
        {
            var path = _configuration.GetSection("Database")?["ConnectionString"]?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + "data";
            }
            return path;
        }

        public bool GetUseDatabase()
        {
            if (bool.TryParse(_configuration.GetSection("Database")?["UseDatabase"]?.ToString() ?? string.Empty, out var useDatabase))
            {
                return useDatabase;
            }
            return false;
        }

        public string GetPDFTemplatePath()
        {
            var path = _configuration.GetSection("PDF")?["templatePath"]?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + "doc\\styles";
            }
            return path;
        }
    }
}