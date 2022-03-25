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
        private readonly IFileService<Core.DTOs.WorkDTO> _fileWorkService;
        private readonly IFileService<Core.Models.Task> _fileTaskService;

        public WorkFileRepository(IFileService<Core.DTOs.WorkDTO> fileWorkService, IFileService<Core.Models.Task> fileTaskService)
        {
            _fileWorkService = fileWorkService;
            _fileTaskService = fileTaskService;
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
                    works.Add(new Core.Models.Work()
                    {
                        Id = work.Id,
                        Date = work.Date,
                        Hours = work.Hours,
                        Task = await _fileTaskService.Get(work.TaskId)
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
                var task = await _fileTaskService.Get(work.TaskId);
                result.Data = new Core.Models.Work()
                {
                    Id = work.Id,
                    Date = work.Date,
                    Hours = work.Hours,
                    Task = task
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
                var works = await _fileWorkService.Create(new Core.DTOs.WorkDTO()
                {
                    Date = entity.Date,
                    Hours = entity.Hours,
                    TaskId = entity.Task.Id
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
                var work = await _fileWorkService.Update(new Core.DTOs.WorkDTO()
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    Hours = entity.Hours,
                    TaskId = entity.Task.Id
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
                result.Data = new Core.Models.Work()
                {
                    Id = work.Id,
                    Date = work.Date,
                    Hours = work.Hours
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