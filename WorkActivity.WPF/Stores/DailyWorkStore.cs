using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Work.Core.Interfaces;

namespace WorkActivity.WPF.Stores
{
    public class DailyWorkStore
    {
        private readonly IDailyWorkService _dailyWorkService;
        private readonly WorkStore _workStore;
        private readonly Lazy<Task> _initialize;
        private readonly List<Work.Core.Models.DailyWork> _dailyWorks;

        public IEnumerable<Work.Core.Models.DailyWork> DailyWorks => _dailyWorks;

        public event Action DailyWorksChanged;

        public DailyWorkStore(IDailyWorkService dailyWorkService, WorkStore workStore)
        {
            _dailyWorkService = dailyWorkService;
            _workStore = workStore;
            _workStore.WorksChanged += WorksChanged;
            _initialize = new Lazy<Task>(Initialize);
            _dailyWorks = new List<Work.Core.Models.DailyWork>();
        }

        private async Task WorksChanged()
        {
            await Initialize();
        }

        private async Task Initialize()
        {
            var dailyWorks = await _dailyWorkService.GetAll();
            _dailyWorks.Clear();
            _dailyWorks.AddRange(dailyWorks);
            DailyWorksChanged?.Invoke();
        }

        public async Task Load()
        {
            await _initialize.Value;
        }
    }
}