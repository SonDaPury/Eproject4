using AutoMapper;
using backend.Base;
using backend.Data;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class LessonService(LMSContext context, IMapper mapper) : ILessonService
    {
        private readonly LMSContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<LessonDto> CreateAsync(LessonDto lessonDto)
        {
            var lesson = _mapper.Map<Lesson>(lessonDto);    
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lessonDto;
        }

        public async Task<(List<LessonDto>,int)> GetAllAsync(Pagination pagination)
        {
            var lessons = await _context.Lessons
                //.Include(l => l.Chapter) // Include the chapter details
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                 .Take(pagination.PageSize)
                .ToListAsync();
            var count = await _context.Lessons.CountAsync();
            return (_mapper.Map<List<LessonDto>>(lessons),count);
        }
        public async Task<List<LessonDto>> GetAllAsync()
        {
            var lessons = await _context.Lessons
                //.Include(l => l.Chapter) // Include the chapter details
                
                .ToListAsync();
            return _mapper.Map<List<LessonDto>>(lessons);
        }
        public async Task<LessonDto?> GetByIdAsync(int id)
        {
            var lesson = await _context.Lessons
                //.Include(l => l.Chapter) // Include the chapter to which the lesson belongs
                .FirstOrDefaultAsync(l => l.Id == id);
            return _mapper?.Map<LessonDto?>(lesson);
        }

        public async Task<LessonDto?> UpdateAsync(int id, LessonDto updatedLesson)
        {
            var update = _mapper.Map<Lesson>(updatedLesson);    
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return null;

            lesson.Title = update.Title;
            lesson.Author = update.Author;
            lesson.VideoDuration = update.VideoDuration;
            lesson.Thumbnail = update.Thumbnail;
            lesson.Video = update.Video;
            lesson.View = update.View;
            lesson.Status = update.Status;
            lesson.ChapterId = update.ChapterId;
            lesson.StaticFolder = update.StaticFolder;

            await _context.SaveChangesAsync();
            return updatedLesson;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return false;

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
