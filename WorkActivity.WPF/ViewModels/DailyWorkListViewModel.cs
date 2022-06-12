using System;
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
        private readonly DailyWorkStore _dailyWorkStore;
        private readonly WorkStore _workStore;
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

        public DailyWorkListViewModel(DailyWorkStore dailyWorkStore,
            WorkStore workStore,
            ParameterNavigationService<object, DailyWorkDetailsListViewModel> detailsNavigationService,
            DailyProgressStore dailyProgressStore)
        {
            _dailyWorkStore = dailyWorkStore;
            _workStore = workStore;
            _detailsNavigationService = detailsNavigationService;
            _dailyProgressStore = dailyProgressStore;

            OnLoadCommand = new RelayCommand(Load);
            ShowDetailsCommand = new RelayCommand(ShowDetails);
        }

        private async void Load(object obj)
        {
            await _dailyWorkStore.Load();
            DailyWorks = _dailyWorkStore.DailyWorks.ToList();
        }

        private void ShowDetails(object sender)
        {
            var dailyWork = sender as DailyWork;
            if (dailyWork != null)
            {
                var works = new List<Work.Core.Models.Work>();
                foreach (var id in dailyWork.WorkIds)
                {
                    var work = _workStore.Works.FirstOrDefault(x => x.Id == id);
                    if (work != null)
                    {
                        works.Add(work);
                    }
                }
                _detailsNavigationService.Navigate(works);
            }
        }
    }
}