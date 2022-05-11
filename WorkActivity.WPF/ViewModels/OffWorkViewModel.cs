using Shared.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Work.Core.Models;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class OffWorkViewModel : ViewModelBase
    {
        private readonly IFileService<OffWork> _offWorkService;
        private readonly NavigationService<AddOffWorkViewModel> _addOffWorkNavigationService;

        public ObservableCollection<OffWorkItemViewModel> OffWorkList { get; set; }

        public ICommand OnLoadCommand { get; }
        public ICommand AddOffWorkCommand { get; }

        public OffWorkViewModel(IFileService<OffWork> offWorkService, NavigationService<AddOffWorkViewModel> addOffWorkNavigationService)
        {
            _offWorkService = offWorkService;
            _addOffWorkNavigationService = addOffWorkNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            AddOffWorkCommand = new NavigateCommand(_addOffWorkNavigationService);

            OffWorkList = new ObservableCollection<OffWorkItemViewModel>();
        }

        private async void Load(object sender)
        {
            var offWorkList = await _offWorkService.GetAll();
            foreach (var offWork in offWorkList)
            {
                OffWorkList.Add(new OffWorkItemViewModel(offWork));
            }
        }
    }
}