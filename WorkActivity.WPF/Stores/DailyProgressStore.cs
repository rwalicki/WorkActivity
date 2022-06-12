using System;
using System.Linq;
using System.Threading.Tasks;

namespace WorkActivity.WPF.Stores
{
    public class DailyProgressStore
    {
        private readonly DailyWorkStore _dailyWorkStore;
        private Lazy<Task> _initialize;
        private float _hours;
        public float Hours
        {
            get => _hours;
            set
            {
                _hours = value;
                ProgressChanged?.Invoke();
            }
        }

        public event Action ProgressChanged;

        public DailyProgressStore(DailyWorkStore dailyWorkStore)
        {
            _dailyWorkStore = dailyWorkStore;
            _dailyWorkStore.DailyWorksChanged += DailyWorksChanged;
            _initialize = new Lazy<Task>(Initialize);
        }

        private async Task Initialize()
        {
            await _dailyWorkStore.Load();
        }

        public async Task Load()
        {
            await _initialize.Value;
        }

        private void DailyWorksChanged()
        {
            var dailyWorks = _dailyWorkStore.DailyWorks;

            var element = dailyWorks.FirstOrDefault();
            if (element != null && element.Date.Date.Equals(DateTime.Today.Date))
            {
                Hours = element?.Hours ?? 0;
            }
            else
            {
                Hours = 0;
            }
        }
    }
}