using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using WkHtmlToPdfDotNet;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Services.Renderer;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class WorkListViewModel : ViewModelBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ISnackbarService _snackbarService;
        private readonly WorkStore _workStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly ParameterNavigationService<object, AddWorkViewModel> _addWorkNavigationService;

        private List<Work.Core.Models.Work> _works;

        private ObservableCollection<string> _filters;
        public IEnumerable<string> Filters => _filters;

        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                ItemView?.Refresh();
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ItemView.Refresh();
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate; set
            {
                _startDate = value; 
                OnPropertyChanged(nameof(StartDate));
                ItemView.Refresh();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate; 
            set
            {
                _endDate = value; 
                OnPropertyChanged(nameof(EndDate));
                ItemView.Refresh();
            }
        }

        public ICollectionView ItemView { get; set; }

        public ICommand OnLoadCommand { get; set; }
        public ICommand AddWorkCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ICommand GeneratePDFCommand { get; }

        public WorkListViewModel(IConfigurationService configurationService, 
            ISnackbarService snackbarService,
            WorkStore workStore,
            ModalNavigationStore modalNavigationStore,
            ParameterNavigationService<object, AddWorkViewModel> addWorkNavigationService)
        {
            _filters = new ObservableCollection<string>();
            _filters.Add("All");
            _filters.Add("Period");
            SelectedFilter = _filters[0];

            _configurationService = configurationService;
            _snackbarService = snackbarService;
            _workStore = workStore;
            _modalNavigationStore = modalNavigationStore;
            _addWorkNavigationService = addWorkNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            AddWorkCommand = new RelayCommand(AddWorkNavigate);
            DeleteCommand = new RelayCommand(Delete);
            GeneratePDFCommand = new RelayCommand(GeneratePDF);
        }

        private bool Filter(object sender)
        {
            var work = sender as Work.Core.Models.Work;
            if (work != null)
            {
                var textCondition = string.IsNullOrEmpty(SearchText) || work.Date.ToString().Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                    work.Task.Title.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                    work.Task.Name.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);

                var dateCondition = SelectedFilter == "All" || (work.Date.Date >= StartDate.Date && work.Date.Date <= EndDate.Date);

                return textCondition && dateCondition;
            }
            return false;
        }

        private async void Load(object obj)
        {
            await _workStore.Load();
            _works = _workStore.Works.OrderByDescending(x => x.Date).ToList();
            
            ItemView = CollectionViewSource.GetDefaultView(_works);
            ItemView.Filter = Filter;
            ItemView.Refresh();
            OnPropertyChanged(nameof(ItemView));

            EndDate = _works.FirstOrDefault()?.Date ?? DateTime.Today.Date;
            StartDate = _works.LastOrDefault()?.Date ?? DateTime.Today.Date;
        }

        private void AddWorkNavigate(object sender)
        {
            _addWorkNavigationService.Navigate(null);
        }

        private void Delete(object sender)
        {
            var work = sender as Work.Core.Models.Work;
            if (work != null)
            {
                var submitCommand = new Action<object>(async (task) =>
                {
                    var result = await _workStore.Delete(work.Id);
                    if (result.Success)
                    {
                        _snackbarService.ShowMessage($"Work id {result.Data.Id} removed.");
                        OnLoadCommand.Execute(null);
                    }
                    _modalNavigationStore.CurrentViewModel = null;
                });
                var cancelCommand = new Action(() => _modalNavigationStore.CurrentViewModel = null);
                _modalNavigationStore.CurrentViewModel = new PopupViewModel($"Do you want to remove work from task {work.Task.Name}?", obj => submitCommand(work), cancelCommand);
            }
        }

        private void GeneratePDF(object obj)
        {
            var works = new List<Work.Core.Models.Work>();
            foreach(var item in ItemView)
            {
                works.Add(item as Work.Core.Models.Work);
            }
            var header = new List<string>()
            {
                "No.", "Id", "Name", "Title", "Date", "Hours"
            };
            var rows = new List<List<string>>();
            var i = 1;
            works = works.OrderBy(x => x.Date).ToList();
            foreach (var work in works)
            {
                rows.Add(new List<string>()
                {
                    $"{i++}", work.Id.ToString(), work.Task.Name, work.Task.Title, work.Date.ToString("dd.MM.yyyy"), work.Hours.ToString()
                });
            }
            var builder = new HTMLTableBuilder().WithHeader(header);
            foreach (var row in rows)
            {
                builder = builder.WithRow(row);
            }
            var table = builder.Build();

            var documentBuilder = new HTMLBuilder(_configurationService.GetPDFTemplatePath()).WithElement(table).Build();

            var converter = new SynchronizedConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = documentBuilder,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            byte[] pdf = converter.Convert(doc);
            var dlg = new SaveFileDialog();
            dlg.InitialDirectory = _configurationService.GetPDFTemplatePath();
            dlg.Filter = "PDF|*.pdf";
            var result = dlg.ShowDialog() ?? false;

            if (result)
            {
                string filename = dlg.FileName;
                FileStream fs = new FileStream(filename, FileMode.CreateNew);
                fs.Write(pdf, 0, pdf.Length);
                fs.Close();
            }
        }
    }
}