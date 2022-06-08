using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public event Func<Task> TasksChanged;

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
                await TasksChanged?.Invoke();
            }
        }

        public async Task Load()
        {
            await _initialize.Value;
        }

        public async Task<ServiceResult<IEnumerable<Work.Core.Models.Task>>> Create(Work.Core.Models.Task task)
        {
            var result = await _taskRepository.Create(task);
            if (result.Success)
            {
                _tasks.Clear();
                _tasks.AddRange(result.Data);
                await TasksChanged?.Invoke();
            }
            return result;
        }

        public async Task<ServiceResult<Work.Core.Models.Task>> Update(Work.Core.Models.Task task)
        {
            var result = await _taskRepository.Update(task);
            if (result.Success)
            {
                _tasks.Remove(task);
                _tasks.Add(result.Data);
                await TasksChanged?.Invoke();
            }
            return result;
        }

        public async Task<ServiceResult<Work.Core.Models.Task>> Delete(int id)
        {
            var result = await _taskRepository.Delete(id);
            if (result.Success)
            {
                var task = _tasks.FirstOrDefault(x => x.Id == id);
                if (task != null)
                {
                    _tasks.Remove(task);
                    await TasksChanged?.Invoke();
                }
            }
            return result;
        }
    }
}