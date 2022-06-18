using System.Threading.Tasks;

namespace WorkActivity.WPF.Services
{
    public interface IPdfService
    {
        Task GeneratePdf(string path, string content);
    }
}