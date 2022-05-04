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
    public class SprintFileRepository : ISprintRepository
    {
        private readonly IFileService<Sprint> _fileRepository;

        public SprintFileRepository(IFileService<Sprint> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<ServiceResult<IEnumerable<Sprint>>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<Sprint>>();
            try
            {
                var sprints = await _fileRepository.GetAll();
                foreach (var sprint in sprints)
                {
                    sprint.IsActive = sprint.StartDate.Date <= DateTime.Today && sprint.EndDate.Date >= DateTime.Today;
                }
                result.Data = sprints;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<Sprint>> Get(int id)
        {
            var result = new ServiceResult<Sprint>();
            try
            {
                var sprint = await _fileRepository.Get(id);
                if (sprint == null)
                {
                    result.Message = "Sprint does not exist.";
                    result.Success = false;
                }
                else
                {
                    sprint.IsActive = sprint.StartDate.Date <= DateTime.Today && sprint.EndDate.Date >= DateTime.Today;
                    result.Data = sprint;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<Sprint>>> Create(Sprint entity)
        {
            var result = new ServiceResult<IEnumerable<Sprint>>();
            try
            {
                var allSprints = await _fileRepository.GetAll();
                var foundSprints = allSprints.Where(x => x.Name == entity.Name);
                if (foundSprints.Any())
                {
                    result.Success = false;
                    result.Message = "Sprint already exists.";
                }
                else
                {
                    var sprints = await _fileRepository.Create(entity);
                    result.Data = sprints;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<Sprint>> Update(Sprint entity)
        {
            var result = new ServiceResult<Sprint>();
            try
            {
                var sprint = await _fileRepository.Update(entity);
                result.Data = sprint;
            }
            catch (Exception ex)
            {

                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<Sprint>> Delete(int id)
        {
            var result = new ServiceResult<Sprint>();
            try
            {
                var sprint = await _fileRepository.Delete(id);
                result.Data = sprint;
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