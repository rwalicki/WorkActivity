using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Work.Core.Interfaces;
using WorkActivity.WPF.Commands;

namespace WorkActivity.WPF.ViewModels
{
    public class AddWorkViewModel : ViewModelBase
    {
        private IWorkRepository _workService;
        private ITaskRepository _taskService;
        private MainWindowViewModel _mainWindowViewModel;

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


        public AddWorkViewModel(IWorkRepository workService, ITaskRepository taskService, MainWindowViewModel mainWindowViewModel, List<Work.Core.Models.Task> tasks, Work.Core.Models.Task task)
        {
            Tasks = tasks;
            Task = task;
            if (Task != null)
            {
                var foundTask = Tasks.FirstOrDefault(x=>x.Id == task.Id);
                var index = Tasks.IndexOf(foundTask);
                SelectedIndex = index;
            }
            _workService = workService;
            _taskService = taskService;
            _mainWindowViewModel = mainWindowViewModel;
            AddTaskCommand = new RelayCommand(async (obj) =>
            {
                await _workService.Create(new Work.Core.Models.Work() { Task = Task, Hours = float.Parse(Hours.Replace('.',',')), Date = Date });
                _mainWindowViewModel.CurrentViewModel = new WorkListViewModel(_workService, _taskService, _mainWindowViewModel);
            });
        }
    }
}