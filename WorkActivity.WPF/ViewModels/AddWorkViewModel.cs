using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;
using WorkActivity.WPF.Services;
using WorkActivity.WPF.Stores;

namespace WorkActivity.WPF.ViewModels
{
    public class AddWorkViewModel : ViewModelBase
    {
        private readonly WorkStore _workStore;
        private readonly TaskStore _taskStore;
        private readonly NavigationService<WorkListViewModel> _workListNavigationService;

        public List<Work.Core.Models.Task> Tasks { get; set; }

        private Work.Core.Models.Task _task;
        public Work.Core.Models.Task Task
        {
            get { return _task; }
            set
            {
                _task = value;
                OnPropertyChanged(nameof(Task));
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        public string _hours = "1";
        public string Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                OnPropertyChanged(nameof(Hours));
            }
        }

        public DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public ICommand AddTaskCommand { get; set; }
        public ICommand OnLoadCommand { get; set; }

        public AddWorkViewModel(WorkStore workStore,
            TaskStore taskStore,
            NavigationService<WorkListViewModel> workListNavigationService,
            object task)
        {
            Tasks = new List<Work.Core.Models.Task>();
            Task = (task as TaskViewModel)?.Task;

            _workStore = workStore;
            _taskStore = taskStore;
            _workListNavigationService = workListNavigationService;

            OnLoadCommand = new RelayCommand(Load);
            AddTaskCommand = new RelayCommand(AddWork);
        }

        private async void Load(object obj)
        {
            await _taskStore.Load();
            var tasks = _taskStore.Tasks.OrderByDescending(x => x.Date).ToList();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }

            if (Task != null)
            {
                var foundTask = Tasks.FirstOrDefault(x => x.Id == Task.Id);
                var index = Tasks.IndexOf(foundTask);
                SelectedIndex = index;
            }

            OnPropertyChanged(nameof(Tasks));
        }

        private async void AddWork(object obj)
        {
            await _workStore.Create(new Work.Core.Models.Work() { Task = Task, Hours = float.Parse(Hours.Replace('.', ',')), Date = Date });
            _workListNavigationService.Navigate();
        }
    }
}