using WorkActivity.WPF.Services;
using WorkActivity.WPF.ViewModels;

namespace WorkActivity.WPF.Commands
{
    public class NavigateCommand : CommandBase
    {
        private INavigationService _navigationService;

        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}