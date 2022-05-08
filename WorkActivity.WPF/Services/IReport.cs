namespace WorkActivity.WPF.Services
{
    public interface IReport
    {
        decimal GetLoggedHours();
        decimal GetExpectedHours();
        decimal GetMissingHours();
    }
}