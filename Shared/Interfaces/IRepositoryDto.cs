using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IRepositoryDto<EntityT, GetT, CreateT, UpdateT>
    {
        Task<ServiceResult<IEnumerable<GetT>>> GetAll();
        Task<ServiceResult<GetT>> GetById(int id);
        Task<ServiceResult<IEnumerable<GetT>>> Create(CreateT entity);
        Task<ServiceResult<GetT>> Update(UpdateT entity);
        Task<ServiceResult<IEnumerable<GetT>>> Delete(int id);
    }
}