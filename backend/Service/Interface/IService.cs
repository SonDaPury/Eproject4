using backend.Base;
using backend.Entities;

namespace backend.Service.Interface
{
    public interface IService<T>
    {
        Task<T> CreateAsync(T chapter);
        Task<(List<T>,int)> GetAllAsync(Pagination pagination);
        Task<T?> GetByIdAsync(int id);
        Task<T?> UpdateAsync(int id, T updatedItem);
        Task<bool> DeleteAsync(int id);
    }
}
