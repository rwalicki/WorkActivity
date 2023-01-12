namespace Work.Core.Interfaces
{
    public interface IConfigurationService
    {
        string GetDatabasePath();
        string GetConnectionString();
        bool GetUseDatabase();
        string GetPDFTemplatePath();
    }
}