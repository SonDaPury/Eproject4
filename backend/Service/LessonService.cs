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

        public async Task<List<Lesson>> GetAllAsync()
        {
            return await _context.Lessons
                //.Include(l => l.Chapter) // Include the chapter details
                .ToListAsync();
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
