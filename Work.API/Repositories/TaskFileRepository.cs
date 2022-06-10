using Shared.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Work.Core.Interfaces;
using Work.Core.Models;

namespace Work.API.Repositories
{
    public class TaskFileRepository : ITaskRepository
    {
        private readonly IFileService<Core.DTOs.Task> _fileTaskRepository;
        private readonly ISprintRepository _sprintRepository;

        public TaskFileRepository(IFileService<Core.DTOs.Task> fileRepository, ISprintRepository sprintRepository)
        {
            _fileTaskRepository = fileRepository;
            _sprintRepository = sprintRepository;
        }

        public async Task<ServiceResult<IEnumerable<Core.Models.Task>>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<Core.Models.Task>>();
            try
            {
                var taskDTOs = await _fileTaskRepository.GetAll();
                var tasks = new List<Core.Models.Task>();

                foreach (var task in taskDTOs)
                {
                    var sprints = new List<Sprint>();
                    if (task.SprintIds != null)
                    {
                        foreach (var sprintId in task.SprintIds)
                        {
                            var sprint = (await _sprintRepository.Get(sprintId)).Data;
                            sprints.Add(sprint);
                        }
                    }

                    tasks.Add(new Core.Models.Task()
                    {
                        Id = task.Id,
                        Date = task.Date,
                        Name = task.Name,
                        Title = task.Title,
                        Sprints = sprints
                    });
                }

                result.Data = tasks;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<Core.Models.Task>> Get(int id)
        {
            var result = new ServiceResult<Core.Models.Task>();
            try
            {
                var taskDTO = await _fileTaskRepository.Get(id);

                var task = new Core.Models.Task()
                {
                    Id = taskDTO.Id,
                    Date = taskDTO.Date,
                    Name = taskDTO.Name,
                    Title = taskDTO.Title,
                    Sprints = new List<Sprint>()
                };

                if (taskDTO.SprintIds != null)
                {
                    foreach (var sprintId in taskDTO.SprintIds)
                    {
                        var sprint = (await _sprintRepository.Get(sprintId)).Data;
                        task.Sprints.Add(sprint);
                    }
                }

                result.Data = task;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<Core.Models.Task>>> Create(Core.Models.Task entity)
        {
            var result = new ServiceResult<IEnumerable<Core.Models.Task>>();
            try
            {
                var allTasks = await _fileTaskRepository.GetAll();
                var foundTasks = allTasks.Where(x => x.Name == entity.Name);
                if (foundTasks.Any())
                {
                    result.Success = false;
                    result.Message = "Task already exists.";
                }
                else
                {
                    var task = new Core.DTOs.Task()
                    {
                        Id = entity.Id,
                        Date = entity.Date,
                        Name = entity.Name,
                        Title = entity.Title,
                        SprintIds = entity.Sprints.Select(x => x.Id).ToList()
                    };

                    await _fileTaskRepository.Create(task);

                    var tasksResult = await GetAll();
                    result.Data = tasksResult.Data;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<Core.Models.Task>> Update(Core.Models.Task entity)
        {
            var result = new ServiceResult<Core.Models.Task>();
            try
            {
                var taskDTO = new Core.DTOs.Task()
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    Name = entity.Name,
                    Title = entity.Title,
                    SprintIds = entity.Sprints.Select(x => x.Id).ToList()
                };

                await _fileTaskRepository.Update(taskDTO);

                var taskResult = await Get(taskDTO.Id);
                result.Data = taskResult.Data;
            }
            catch (Exception ex)
            {

                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<Core.Models.Task>> Delete(int id)
        {
            var result = new ServiceResult<Core.Models.Task>();
            try
            {
                var taskDTO = await _fileTaskRepository.Delete(id);
                var task = new Core.Models.Task()
                {
                    Id = taskDTO.Id,
                    Date = taskDTO.Date,
                    Name = taskDTO.Name,
                    Title = taskDTO.Title
                };
                result.Data = task;
            }
            catch (Exception ex)
            {

                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }
    }
}