using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using Work.Core.Models;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class DailyWorkListViewModel : ViewModelBase
    {
        private readonly IDailyWorkService _dailyWorkService;

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
        public ICommand OnShowDetailsCommand { get; set; }

        public DailyWorkListViewModel(IDailyWorkService dailyWorkService)
        {
            _dailyWorkService = dailyWorkService;

            OnLoadCommand = new RelayCommand(async (obj) =>
            {
                DailyWorks = (await _dailyWorkService.GetAll()).ToList();
            });

            //OnShowDetailsCommand = new RelayCommand(async (obj) =>
            //{
            //    var dailyWork = obj as DailyWork;
            //    if (dailyWork != null)
            //    {
            //        var works = new List<Work.Core.Models.Work>();
            //        foreach (var id in dailyWork.WorkIds)
            //        {
            //            var result = await _workService.Get(id);
            //            if (result.Success)
            //            {
            //                works.Add(result.Data);
            //            }
            //        }
            //        _navigationStore.CurrentViewModel = new DailyWorkDetailsListViewModel(_workService, works);
            //        (_mainWindowViewModel.CurrentViewModel as DailyWorkDetailsListViewModel)?.OnLoadCommand.Execute(null);
            //    }
            //});
        }
    }
}