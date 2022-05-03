using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Work.Core.Interfaces;

namespace WorkActivity.WPF.Stores
{
    public class TaskStore
    {
        private readonly ITaskRepository _taskRepository;
        private readonly Lazy<Task> _initialize;
        private readonly List<Work.Core.Models.Task> _tasks;

        public IEnumerable<Work.Core.Models.Task> Tasks => _tasks;

        public TaskStore(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            _initialize = new Lazy<Task>(Initialize);
            _tasks = new List<Work.Core.Models.Task>();
        }

        private async Task Initialize()
        {
            var result = await _taskRepository.GetAll();
            if (result.Success)
            {
                _tasks.Clear();
                _tasks.AddRange(result.Data);
            }
        } 

        public async Task Load()
        {
            await _initialize.Value;
        }
    }
}