using backend.Entities;

namespace backend.Service.Interface
{
    public interface IExamService : IService<Exam>
    {
        Task EndExam(int examId, int userId);
        Task<dynamic> GetDetailsExam(int examId);
        Task StartExam(int examId, int userId);
    }
}
