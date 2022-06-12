namespace WorkActivity.WPF.Services
{
    public interface IReport
    {
        decimal GetLoggedHours(int month, int year);
        decimal GetExpectedHours(int month, int year);
        decimal GetMissingHours(int month, int year);
    }
}