using backend.Entities;

namespace backend.Service.Interface
{
    public interface IChapterService : IService<Chapter>
    {
        Task<object> GetChapterBySourceID(int sourceID);
    }
}
