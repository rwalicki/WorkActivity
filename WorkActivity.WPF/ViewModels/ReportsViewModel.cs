using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Services.Renderer;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly IReport _reportService;
        private readonly WorkStore _workStore;

        private readonly ObservableCollection<DateTime> _months;
        public IEnumerable<DateTime> Months => _months;

        private DateTime _selectedMonth;
        public DateTime SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged(nameof(SelectedMonth));
            }
        }

        private decimal _expectedHours;
        public decimal ExpectedHours
        {
            get => _expectedHours;
            set
            {
                _expectedHours = value;
                OnPropertyChanged(nameof(ExpectedHours));
            }
        }

        private decimal _loggedHours;
        public decimal LoggedHours
        {
            get => _loggedHours;
            set
            {
                _loggedHours = value;
                OnPropertyChanged(nameof(LoggedHours));
            }
        }

        private decimal _missingHours;
        public decimal MissingHours
        {
            get => _missingHours;
            set
            {
                _missingHours = value;
                OnPropertyChanged(nameof(MissingHours));
            }
        }

        public ICommand OnLoadCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand GenerateCommand { get; }

        public ReportsViewModel(IReport reportService, WorkStore workStore)
        {
            _months = new ObservableCollection<DateTime>();

            _reportService = reportService;
            _workStore = workStore;

            OnLoadCommand = new RelayCommand(s => Load(s));
            GenerateReportCommand = new RelayCommand(async s => await GenerateReport(SelectedMonth.Month, SelectedMonth.Year));
            GenerateCommand = new RelayCommand(Generate);
        }

        private void Generate(object obj)
        {
            var _works = _workStore.Works;
            var header = new List<string>()
            {
                "Id", "Name", "Title", "Date", "Hours"
            };
            var rows = new List<List<string>>();
            foreach (var work in _works) 
            {
                rows.Add(new List<string>()
                {
                    work.Id.ToString(), work.Task.Name, work.Task.Title, work.Date.ToString("dd.MM.yyyy"), work.Hours.ToString()
                });
            }
            var builder = new HTMLTableBuilder().WithHeader(header);
            foreach(var row in rows)
            {
                builder = builder.WithRow(row);
            }
            var strin = builder.Build();

        }

        private async void Load(object sender)
        {
            await _workStore.Load();
            var works = _workStore.Works;
            var dates = new List<DateTime>();
            foreach (var work in works)
            {
                var date = new DateTime(work.Date.Year, work.Date.Month, 1);
                if (!dates.Exists(x => x.Month.Equals(date.Month) && x.Year.Equals(date.Year)))
                {
                    dates.Add(date);
                }
            }
            dates = dates.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).ToList();

            dates.ForEach(x => _months.Add(x));

            var actualDate = DateTime.Today;
            var element = Months.FirstOrDefault(x => x.Month.Equals(actualDate.Month) && x.Year.Equals(actualDate.Year));
            if (element != DateTime.MinValue)
            {
                SelectedMonth = element;
                await GenerateReport(SelectedMonth.Month, SelectedMonth.Year);
            }
        }

        private async Task GenerateReport(int month, int year)
        {
            await Task.Run(() =>
            {
                ExpectedHours = _reportService.GetExpectedHours(month, year);
                LoggedHours = _reportService.GetLoggedHours(month, year);
                MissingHours = _reportService.GetMissingHours(month, year);
            });
        }
    }
}