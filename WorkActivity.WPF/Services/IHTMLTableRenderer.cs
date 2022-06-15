namespace WorkActivity.WPF.Services
{
    public interface IHTMLTableRenderer
    {
        void AddHeader(string[] header);
        void AddRow(string[] row);
        string Render();
    }
}