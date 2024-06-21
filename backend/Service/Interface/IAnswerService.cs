using backend.Entities;

namespace backend.Service.Interface
{
    public interface IAnswerService : IService<Answer>
    {
        Task<object> GetAllByUserIdAndExamId(int userId, int examId);
    }
}
