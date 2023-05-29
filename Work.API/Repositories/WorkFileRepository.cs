using Shared.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Work.Core.Interfaces;

namespace Work.API.Repositories
{
    public class WorkFileRepository : IWorkRepository
    {
        private readonly IFileService<Core.DTOs.Work> _fileWorkService;
        private readonly ITaskRepository _taskRepository;

        public WorkFileRepository(IFileService<Core.DTOs.Work> fileWorkService, ITaskRepository taskRepository)
        {
            _fileWorkService = fileWorkService;
            _taskRepository = taskRepository;
        }

        public async Task<ServiceResult<IEnumerable<Core.Models.Work>>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<Core.Models.Work>>();
            try
            {
                var works = new List<Core.Models.Work>();
                var workDTOs = await _fileWorkService.GetAll();
                foreach (var work in workDTOs)
                {
                    var task = (await _taskRepository.Get(work.TaskId)).Data;
                    works.Add(new Core.Models.Work()
                    {
                        Id = work.Id,
                        Date = work.Date,
                        Hours = work.Hours,
                        Task = task,
                        IsOverWork = work.IsOverWork
                    });
                }
                result.Data = works;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<Core.Models.Work>> Get(int id)
        {
            var result = new ServiceResult<Core.Models.Work>();
            try
            {
                var work = await _fileWorkService.Get(id);
                var task = (await _taskRepository.Get(work.TaskId)).Data;
                result.Data = new Core.Models.Work()
                {
                    Id = work.Id,
                    Date = work.Date,
                    Hours = work.Hours,
                    Task = task,
                    IsOverWork = work.IsOverWork
                };
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<Core.Models.Work>>> Create(Core.Models.Work entity)
        {
            var result = new ServiceResult<IEnumerable<Core.Models.Work>>();
            try
            {
                var works = await _fileWorkService.Create(new Core.DTOs.Work()
                {
                    Date = entity.Date,
                    Hours = entity.Hours,
                    TaskId = entity.Task.Id,
                    IsOverWork = entity.IsOverWork
                });
                result.Data = (await GetAll()).Data;
            }
            catch (Exception ex)
            {

                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<Core.Models.Work>> Update(Core.Models.Work entity)
        {
            var result = new ServiceResult<Core.Models.Work>();
            try
            {
                var work = await _fileWorkService.Update(new Core.DTOs.Work()
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    Hours = entity.Hours,
                    TaskId = entity.Task.Id,
                    IsOverWork = entity.IsOverWork
                });
                result.Data = (await Get(entity.Id)).Data;
            }
            catch (Exception ex)
            {

                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<Core.Models.Work>> Delete(int id)
        {
            var result = new ServiceResult<Core.Models.Work>();
            try
            {
                var work = await _fileWorkService.Delete(id);
                var task = (await _taskRepository.Get(work.TaskId)).Data;
                result.Data = new Core.Models.Work()
                {
                    Id = work.Id,
                    Date = work.Date,
                    Hours = work.Hours,
                    Task = task,
                    IsOverWork = work.IsOverWork
                };
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