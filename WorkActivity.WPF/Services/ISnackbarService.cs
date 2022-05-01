using MaterialDesignThemes.Wpf;

namespace WorkActivity.WPF.Services
{
    public interface ISnackbarService
    {
        SnackbarMessageQueue GetSnackbar();
        void ShowMessage(string message);
    }
}