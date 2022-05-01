using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IWorkRepository _workService;
        private readonly ITaskRepository _taskService;
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


        public AddWorkViewModel(IWorkRepository workService, 
            ITaskRepository taskService, 
            NavigationService<WorkListViewModel> workListNavigationService)
        {
            Tasks = new List<Work.Core.Models.Task>();
            //Task = task;
            //if (Task != null)
            //{
            //    var foundTask = Tasks.FirstOrDefault(x => x.Id == task.Id);
            //    var index = Tasks.IndexOf(foundTask);
            //    SelectedIndex = index;
            //}
            _workService = workService;
            _taskService = taskService;
            _workListNavigationService = workListNavigationService;
            OnLoadCommand = new RelayCommand(Load);
            AddTaskCommand = new RelayCommand(AddWork);
        }

        private async void AddWork(object obj)
        {
            await _workService.Create(new Work.Core.Models.Work() { Task = Task, Hours = float.Parse(Hours.Replace('.', ',')), Date = Date });
            _workListNavigationService.Navigate();
        }

        private async void Load(object obj)
        {
            var result = await _taskService.GetAll();
            if (result.Success)
            {
                foreach(var task in result.Data)
                {
                    Tasks.Add(task);
                }
                OnPropertyChanged(nameof(Tasks));
            }
        }
    }
}