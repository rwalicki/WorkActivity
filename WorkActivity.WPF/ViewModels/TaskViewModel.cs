using System;
using System.Linq;
using Work.Core.Models;

namespace WorkActivity.WPF.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private Task _task;
        public Task Task => _task;

        public int Id => _task.Id;

        public string Title => _task.Title;

        public string Number => _task.Number.ToString();

        public DateTime Date => _task.Date.Date;

        public string Sprints
        {
            get
            {
                return string.Join(", ", _task.Sprints.Select(x => x.Name).ToList());
            }
        }

        public TaskViewModel(Task task)
        {
            _task = task;
        }
    }
}