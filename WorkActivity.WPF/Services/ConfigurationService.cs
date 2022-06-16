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
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
            return path;
        }

        public string GetPDFTemplatePath()
        {
            var path = _configuration.GetSection("PDF")?["templatePath"]?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
            return path;
        }
    }
}