using backend.Entities;

namespace backend.Service.Interface
{
    public interface ISourceService : IService<Source>
    {
        Task<List<SourceWithTopicId>> GetAllAsync();
        Task<SourceWithTopicId?> GetByIdAsync(int id);
    }
}
