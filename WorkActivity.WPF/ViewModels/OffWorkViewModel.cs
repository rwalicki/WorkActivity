using Shared.Interfaces;
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

        public ObservableCollection<OffWorkItemViewModel> OffWorkList { get; set; }

        public ICommand OnLoadCommand { get; }
        public ICommand AddOffWorkCommand { get; }
        public ICommand DeleteCommand { get; }

        public OffWorkViewModel(IOffWorkRepository offWorkService, NavigationService<AddOffWorkViewModel> addOffWorkNavigationService)
        {
            _offWorkRepository = offWorkService;
            _addOffWorkNavigationService = addOffWorkNavigationService;

            OnLoadCommand = new RelayCommand(async (sender) => await Load(sender));
            AddOffWorkCommand = new NavigateCommand(_addOffWorkNavigationService);
            DeleteCommand = new RelayCommand(async (sender) => await Delete(sender));
        }

        private async Task Load(object sender)
        {
            OffWorkList = new ObservableCollection<OffWorkItemViewModel>();

            var offWorkList = await _offWorkRepository.GetAll();
            if (offWorkList.Success)
            {
                foreach (var offWork in offWorkList.Data)
                {
                    OffWorkList.Add(new OffWorkItemViewModel(offWork));
                }
            }

            OnPropertyChanged(nameof(OffWorkList));
        }

        private async Task Delete(object sender)
        {
            var offWorkItem = sender as OffWorkItemViewModel;
            await _offWorkRepository.Delete(offWorkItem.OffWork.Id);
            await Load(sender);
        }
    }
}