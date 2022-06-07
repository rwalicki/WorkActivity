using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using Work.Core.Models;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class DailyWorkListViewModel : ViewModelBase
    {
        private readonly IDailyWorkService _dailyWorkService;
        private readonly IWorkRepository _workRepository;
        private readonly ParameterNavigationService<object, DailyWorkDetailsListViewModel> _detailsNavigationService;
        private readonly DailyProgressStore _dailyProgressStore;

        private List<DailyWork> _dailyWorks;
        public List<DailyWork> DailyWorks
        {
            get { return _dailyWorks; }
            set
            {
                _dailyWorks = value;
                OnPropertyChanged(nameof(DailyWorks));
            }
        }

        public ICommand OnLoadCommand { get; set; }
        public ICommand OnSelectItem { get; set; }
        public ICommand ShowDetailsCommand { get; set; }

        public DailyWorkListViewModel(IDailyWorkService dailyWorkService,
            IWorkRepository workRepository,
            ParameterNavigationService<object, DailyWorkDetailsListViewModel> detailsNavigationService,
            DailyProgressStore dailyProgressStore)
        {
            _dailyWorkService = dailyWorkService;
            _workRepository = workRepository;
            _detailsNavigationService = detailsNavigationService;
            _dailyProgressStore = dailyProgressStore;

            OnLoadCommand = new RelayCommand(Load);
            ShowDetailsCommand = new RelayCommand(ShowDetails);
        }

        private async void Load(object obj)
        {
            DailyWorks = (await _dailyWorkService.GetAll()).ToList();
            var element = DailyWorks.FirstOrDefault();
            _dailyProgressStore.Hours = element?.Hours ?? 0;
        }

        private async void ShowDetails(object sender)
        {
            var dailyWork = sender as DailyWork;
            if (dailyWork != null)
            {
                var works = new List<Work.Core.Models.Work>();
                foreach (var id in dailyWork.WorkIds)
                {
                    var result = await _workRepository.Get(id);
                    if (result.Success)
                    {
                        works.Add(result.Data);
                    }
                }
                _detailsNavigationService.Navigate(works);
            }
        }
    }
}