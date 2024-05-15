using backend.Entities;

namespace backend.Service.Interface
{
    public interface IExamService : IService<Exam>
    {
        Task<int> EndExam(List<UserAnswer> userAnswers, int examId, int userId);
        Task<dynamic> GetDetailsExam(int examId);
        Task StartExam(int examId, int userId);
    }
}
