using backend.Base;
using backend.Dtos;
using backend.Entities;

namespace backend.Service.Interface
{
    public interface IQuestionService
    {
        Task<QuestionDto> CreateAsync(QuestionDto questionDto);
        Task<bool> DeleteAsync(int id);
        Task<List<QuestionDto>> GetAllAsync();
        Task<(List<QuestionDto>, int)> GetAllAsync(Pagination pagination);
        Task<QuestionDto?> GetByIdAsync(int id);
        Task<QuestionDto?> UpdateAsync(int id, QuestionDto updatedQuestion);
    }
}
