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
        private readonly IWorkRepository _workService;
        private readonly MainWindowViewModel _mainWindowViewModel;

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

        public DailyWorkListViewModel(IDailyWorkService dailyWorkService, IWorkRepository workService, MainWindowViewModel mainWindowViewModel)
        {
            _dailyWorkService = dailyWorkService;
            _workService = workService;
            _mainWindowViewModel = mainWindowViewModel;

            OnLoadCommand = new RelayCommand(async (obj) =>
            {
                DailyWorks = (await _dailyWorkService.GetAll()).ToList();
            });

            OnShowDetailsCommand = new RelayCommand(async (obj) =>
            {
                var dailyWork = obj as DailyWork;
                if (dailyWork != null)
                {
                    var works = new List<Work.Core.Models.Work>();
                    foreach (var id in dailyWork.WorkIds)
                    {
                        var result = await _workService.Get(id);
                        if (result.Success)
                        {
                            works.Add(result.Data);
                        }
                    }
                    _mainWindowViewModel.CurrentViewModel = new DailyWorkDetailsListViewModel(_workService, works);
                    (_mainWindowViewModel.CurrentViewModel as DailyWorkDetailsListViewModel)?.OnLoadCommand.Execute(null);
                }
            });
        }
    }
}