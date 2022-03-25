using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IRepository<T>
    {
        Task<ServiceResult<IEnumerable<T>>> GetAll();
        Task<ServiceResult<T>> Get(int id);
        Task<ServiceResult<IEnumerable<T>>> Create(T entity);
        Task<ServiceResult<T>> Update(T entity);
        Task<ServiceResult<T>> Delete(int id);
    }
}