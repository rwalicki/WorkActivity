namespace Work.Core.Interfaces
{
    public interface IConfigurationService
    {
        string GetDatabasePath();
        string GetPDFTemplatePath();
    }
}