using backend.Base;
using backend.Dtos;
using backend.Entities;

namespace backend.Service.Interface
{
    public interface ILessonService
    {
        Task<LessonDto> CreateAsync(LessonDto lessonDto);
        Task<bool> DeleteAsync(int id);
        Task<(List<LessonDto>, int)> GetAllAsync(Pagination pagination);
        Task<object> GetAllAsync(int chapterID);
        Task<LessonDto?> GetByIdAsync(int id);
        Task<LessonDto?> UpdateAsync(int id, LessonDto updatedLesson);
    }
}
