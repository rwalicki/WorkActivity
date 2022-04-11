using Shared.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Work.Core.Interfaces;

namespace Work.API.Repositories
{
    public class TaskFileRepository : ITaskRepository
    {
        private readonly IFileService<Core.DTOs.Task> _fileRepository;
        private readonly ISprintService _sprintService;

        public TaskFileRepository(IFileService<Core.DTOs.Task> fileRepository, ISprintService sprintService)
        {
            _fileRepository = fileRepository;
            _sprintService = sprintService;
        }

        public async Task<ServiceResult<IEnumerable<Core.Models.Task>>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<Core.Models.Task>>();
            try
            {
                var taskDTOs = await _fileRepository.GetAll();
                var tasks = new List<Core.Models.Task>();

                foreach(var task in taskDTOs)
                {
                    var sprints = task.SprintIds
                        .Select(async x => (await _sprintService.Get(x)).Data)
                        .Select(x=>x.Result)
                        .ToList();

                    tasks.Add(new Core.Models.Task()
                    {
                        Id = task.Id,
                        Date = task.Date,
                        Number = task.Number,
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
                var taskDTO = await _fileRepository.Get(id);
                var sprints = taskDTO.SprintIds
                        .Select(async x => (await _sprintService.Get(x)).Data)
                        .Select(x => x.Result)
                        .ToList();

                var task = new Core.Models.Task()
                {
                    Id = taskDTO.Id,
                    Date = taskDTO.Date,
                    Number = taskDTO.Number,
                    Title = taskDTO.Title,
                    Sprints = sprints
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

        public async Task<ServiceResult<IEnumerable<Core.Models.Task>>> Create(Core.Models.Task entity)
        {
            var result = new ServiceResult<IEnumerable<Core.Models.Task>>();
            try
            {
                var allTasks = await _fileRepository.GetAll();
                var foundTasks = allTasks.Where(x=>x.Number == entity.Number);
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
                        Number = entity.Number,
                        Title = entity.Title,
                        SprintIds = entity.Sprints.Select(x => x.Id).ToList()
                    };
                    var taskDTOs = await _fileRepository.Create(task);
                    
                    var tasks = new List<Core.Models.Task>();

                    foreach (var taskDTO in taskDTOs)
                    {
                        var sprints = taskDTO.SprintIds
                            .Select(async x => (await _sprintService.Get(x)).Data)
                            .Select(x => x.Result)
                            .ToList();

                        tasks.Add(new Core.Models.Task()
                        {
                            Id = taskDTO.Id,
                            Date = taskDTO.Date,
                            Number = taskDTO.Number,
                            Title = taskDTO.Title,
                            Sprints = sprints
                        });
                    }
                    result.Data = tasks;
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
                    Number = entity.Number,
                    Title = entity.Title,
                    SprintIds = entity.Sprints.Select(x => x.Id).ToList()
                };
                
                taskDTO = await _fileRepository.Update(taskDTO);
                var sprints = taskDTO.SprintIds
                        .Select(async x => (await _sprintService.Get(x)).Data)
                        .Select(x => x.Result)
                        .ToList();

                var task = new Core.Models.Task()
                {
                    Id = taskDTO.Id,
                    Date = taskDTO.Date,
                    Number = taskDTO.Number,
                    Title = taskDTO.Title,
                    Sprints = sprints
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

        public async Task<ServiceResult<Core.Models.Task>> Delete(int id)
        {
            var result = new ServiceResult<Core.Models.Task>();
            try
            {
                var taskDTO = await _fileRepository.Delete(id);
                var task = new Core.Models.Task()
                {
                    Id = taskDTO.Id,
                    Date = taskDTO.Date,
                    Number = taskDTO.Number,
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