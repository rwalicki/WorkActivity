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
        private readonly IFileService<Core.Models.Task> _fileRepository;

        public TaskFileRepository(IFileService<Core.Models.Task> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<ServiceResult<IEnumerable<Core.Models.Task>>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<Core.Models.Task>>();
            try
            {
                var tasks = await _fileRepository.GetAll();
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
                var task = await _fileRepository.Get(id);
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
                    var tasks = await _fileRepository.Create(entity);
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
                var task = await _fileRepository.Update(entity);
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
                var task = await _fileRepository.Delete(id);
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