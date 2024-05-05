using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class ChapterService(LMSContext context) : IChapterService
    {
        private readonly LMSContext _context = context;

        public async Task<Chapter> CreateAsync(Chapter chapter)
        {
            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();
            return chapter;
        }

        public async Task<List<Chapter>> GetAllAsync()
        {
            return await _context.Chapters
                //.Include(c => c.Source) // Includes the source of the chapter
                //.Include(c => c.Lessions) // Includes all lessons in the chapter
                //.Include(c => c.Exams) // Includes all exams in the chapter
                .ToListAsync();
        }

        public async Task<Chapter?> GetByIdAsync(int id)
        {
            return await _context.Chapters
                //.Include(c => c.Source)
                //.Include(c => c.Lessions)
                //.Include(c => c.Exams)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Chapter?> UpdateAsync(int id, Chapter updatedChapter)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null) return null;

            chapter.Title = updatedChapter.Title;
            chapter.Description = updatedChapter.Description;
            //chapter.SourceId = updatedChapter.SourceId;
            // This operation does not handle changes to relations like Lessions or Exams, which might require additional logic

            await _context.SaveChangesAsync();
            return chapter;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null) return false;
            // xóa lession theo chapter
            var list_lession = await _context.Lessons.Where(l => l.ChapterId == id).ToListAsync();
            if (list_lession != null)
            {
                _context.Lessons.RemoveRange(list_lession);
            }

            // xóa exam theo chapter
            var list_exam = await _context.Exams.Where(l => l.ChapterId == id).ToListAsync();
            if (list_exam != null)
            {
                _context.Exams.RemoveRange(list_exam);
            }
            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
