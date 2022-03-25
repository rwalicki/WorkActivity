using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class WorkListViewModel : ViewModelBase
    {
        private readonly IWorkRepository _workService;
        private readonly ITaskRepository _taskService;
        private readonly MainWindowViewModel _mainWindowViewModel;

        private List<Work.Core.Models.Work> _works;

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

        public ICollectionView ItemView { get; set; }

        public ICommand OnLoadCommand { get; set; }
        public ICommand AddWorkCommand { get; set; }
        public ICommand OnDeleteCommand { get; set; }

        public WorkListViewModel(IWorkRepository workService, ITaskRepository taskService, MainWindowViewModel mainWindowViewModel)
        {
            _workService = workService;
            _taskService = taskService;
            _mainWindowViewModel = mainWindowViewModel;

            OnLoadCommand = new RelayCommand(async (obj) =>
            {
                var result = await _workService.GetAll();
                if (result.Success)
                {
                    _works = result.Data.OrderByDescending(x=>x.Date).ToList();
                    ItemView = CollectionViewSource.GetDefaultView(_works);
                    ItemView.Filter = Filter;
                    ItemView.Refresh();
                    OnPropertyChanged(nameof(ItemView));
                }
            });

            AddWorkCommand = new RelayCommand(async (obj) =>
            {
                var tasks = (await _taskService.GetAll()).Data.OrderByDescending(x => x.Date).ToList();
                _mainWindowViewModel.CurrentViewModel = new AddWorkViewModel(_workService, _taskService, _mainWindowViewModel, tasks, null);
            });

            OnDeleteCommand = new RelayCommand(async (obj) =>
            {
                var work = obj as Work.Core.Models.Work;
                if (work != null)
                {
                    var result = await _workService.Delete(work.Id);
                    if (result.Success)
                    {
                        _mainWindowViewModel.SnakbarMessageQueue.Enqueue($"Work id {result.Data.Id} removed.");
                        OnLoadCommand.Execute(null);
                    }
                }
            });
        }

        private bool Filter(object sender)
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return true;
            }

            var work = sender as Work.Core.Models.Work;
            if (work != null)
            {
                return work.Date.ToString().Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) || 
                    work.Task.Title.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                    work.Task.Number.ToString().Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
    }
}