using MaterialDesignThemes.Wpf;

namespace WorkActivity.WPF.Services
{
    public class SnackbarService : ISnackbarService
    {
        private SnackbarMessageQueue _snackbarMessageQueue;

        public SnackbarService()
        {
            _snackbarMessageQueue = new SnackbarMessageQueue();
        }

        public SnackbarMessageQueue GetSnackbar()
        {
            return _snackbarMessageQueue;
        }

        public void ShowMessage(string message)
        {
            _snackbarMessageQueue.Enqueue(message);
        }
    }
}