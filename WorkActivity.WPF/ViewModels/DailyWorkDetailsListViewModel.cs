using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class DailyWorkDetailsListViewModel : ViewModelBase
    {
        private readonly IWorkRepository _workService;

        private List<Work.Core.Models.Work> _dailyWorks;

        private ObservableCollection<Work.Core.Models.Work> _works;
        public ObservableCollection<Work.Core.Models.Work> Works
        {
            get { return _works; }
            set
            {
                _works = value;
                OnPropertyChanged(nameof(Works));
            }
        }

        public ICommand OnDeleteCommand { get; set; }
        public ICommand OnLoadCommand { get; set; }

        public DailyWorkDetailsListViewModel(IWorkRepository workService, List<Work.Core.Models.Work> works)
        {
            _workService = workService;
            _dailyWorks = works;

            OnLoadCommand = new RelayCommand((obj) =>
            {
                Works = new ObservableCollection<Work.Core.Models.Work>(_dailyWorks);
            });

            OnDeleteCommand = new RelayCommand(async (obj) =>
            {
                var work = obj as Work.Core.Models.Work;
                var result = await _workService.Delete(work.Id);
                if (result.Success)
                {
                    Works.Remove(Works.Where(x => x.Id == result.Data.Id).First());
                }
            });
        }
    }
}