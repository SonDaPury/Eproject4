using backend.Base;
using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class LessonService(LMSContext context) : ILessonService
    {
        private readonly LMSContext _context = context;

        public async Task<Lesson> CreateAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<(List<Lesson>,int)> GetAllAsync(Pagination pagination)
        {
            var lessons = await _context.Lessons
                //.Include(l => l.Chapter) // Include the chapter details
                 .Take(pagination.PageSize)
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .ToListAsync();
            var count = await _context.Lessons.CountAsync();
            return (lessons,count);
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await _context.Lessons
                //.Include(l => l.Chapter) // Include the chapter to which the lesson belongs
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Lesson?> UpdateAsync(int id, Lesson updatedLesson)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return null;

            lesson.Title = updatedLesson.Title;
            lesson.Author = updatedLesson.Author;
            lesson.VideoDuration = updatedLesson.VideoDuration;
            lesson.Thumbnail = updatedLesson.Thumbnail;
            lesson.Video = updatedLesson.Video;
            lesson.View = updatedLesson.View;
            lesson.Status = updatedLesson.Status;
            lesson.ChapterId = updatedLesson.ChapterId;
            lesson.StaticFolder = updatedLesson.StaticFolder;

            await _context.SaveChangesAsync();
            return lesson;
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
