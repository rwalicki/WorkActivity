using Shared.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Work.Core.Interfaces;
using Work.Core.Models;

namespace Work.API.Repositories
{
    public class OffWorkRepository : IOffWorkRepository
    {
        private readonly IFileService<OffWork> _fileRepository;

        public OffWorkRepository(IFileService<OffWork> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<ServiceResult<IEnumerable<OffWork>>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<OffWork>>();
            try
            {
                var offWorks = await _fileRepository.GetAll();
                result.Data = offWorks;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<OffWork>> Get(int id)
        {
            var result = new ServiceResult<OffWork>();
            try
            {
                var offWork = await _fileRepository.Get(id);
                if (offWork == null)
                {
                    result.Message = "Off Work does not exist.";
                    result.Success = false;
                }
                else
                {
                    result.Data = offWork;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<IEnumerable<OffWork>>> Create(OffWork entity)
        {
            var result = new ServiceResult<IEnumerable<OffWork>>();
            try
            {
                var offWork = await _fileRepository.Create(entity);
                result.Data = offWork;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<OffWork>> Update(OffWork entity)
        {
            var result = new ServiceResult<OffWork>();
            try
            {
                var offWork = await _fileRepository.Update(entity);
                result.Data = offWork;
            }
            catch (Exception ex)
            {

                result.Message = ex.ToString();
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResult<OffWork>> Delete(int id)
        {
            var result = new ServiceResult<OffWork>();
            try
            {
                var offWork = await _fileRepository.Delete(id);
                result.Data = offWork;
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