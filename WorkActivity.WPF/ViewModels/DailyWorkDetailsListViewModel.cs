using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class DailyWorkDetailsListViewModel : ViewModelBase
    {
        private readonly WorkStore _workStore;

        private List<Work.Core.Models.Work> _dailyWorks;

        private readonly ObservableCollection<Work.Core.Models.Work> _works;
        public IEnumerable<Work.Core.Models.Work> Works => _works;

        public ICommand OnLoadCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public DailyWorkDetailsListViewModel(WorkStore workStore, object dailyWorks)
        {
            _works = new ObservableCollection<Work.Core.Models.Work>();

            _workStore = workStore;
            _dailyWorks = dailyWorks as List<Work.Core.Models.Work>;

            OnLoadCommand = new RelayCommand(Load);
            DeleteCommand = new RelayCommand(Delete);
        }

        private void Load(object obj)
        {
            foreach(var dailyWork in _dailyWorks)
            {
                _works.Add(dailyWork);
            }
        }

        private async void Delete(object sender)
        {
            var work = sender as Work.Core.Models.Work;
            var result = await _workStore.Delete(work.Id);
            if (result.Success)
            {
                _works.Remove(_works.Where(x => x.Id == result.Data.Id).First());
            }
        }
    }
}