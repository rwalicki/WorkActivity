using Shared.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;

namespace WorkActivity.WPF.ViewModels
{
    public class OffWorkViewModel : ViewModelBase
    {
        private readonly IOffWorkRepository _offWorkRepository;
        private readonly NavigationService<AddOffWorkViewModel> _addOffWorkNavigationService;

        private readonly ObservableCollection<OffWorkItemViewModel> _offWorkList;
        public IEnumerable<OffWorkItemViewModel> OffWorkList => _offWorkList;

        public ICommand OnLoadCommand { get; }
        public ICommand AddOffWorkCommand { get; }
        public ICommand DeleteCommand { get; }

        public OffWorkViewModel(IOffWorkRepository offWorkService, NavigationService<AddOffWorkViewModel> addOffWorkNavigationService)
        {
            _offWorkList = new ObservableCollection<OffWorkItemViewModel>();

            _offWorkRepository = offWorkService;
            _addOffWorkNavigationService = addOffWorkNavigationService;

            OnLoadCommand = new RelayCommand(async (sender) => await Load(sender));
            AddOffWorkCommand = new NavigateCommand(_addOffWorkNavigationService);
            DeleteCommand = new RelayCommand(async (sender) => await Delete(sender));
        }

        private async Task Load(object sender)
        {
            _offWorkList.Clear();

            var offWorkList = await _offWorkRepository.GetAll();
            if (offWorkList.Success)
            {
                foreach (var offWork in offWorkList.Data)
                {
                    _offWorkList.Add(new OffWorkItemViewModel(offWork));
                }
            }

            OnPropertyChanged(nameof(OffWorkList));
        }

        private async Task Delete(object sender)
        {
            var offWorkItem = sender as OffWorkItemViewModel;
            var result = await _offWorkRepository.Delete(offWorkItem.OffWork.Id);
            if (result.Success)
            {
                await Load(sender);
            }
        }
    }
}