using backend.Entities;

namespace backend.Service.Interface
{
    public interface ISourceService : IService<Source>
    {
        Task<SourceWithTopicId?> GetByIdAsync(int id);
    }
}
