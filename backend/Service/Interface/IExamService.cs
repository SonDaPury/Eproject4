using backend.Entities;

namespace backend.Service.Interface
{
    public interface IExamService : IService<Exam>
    {
        Task EndExam(int examId, int userId);
        Task StartExam(int examId, int userId);
    }
}
