using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class ExamService(LMSContext context) : IExamService
    {
        private readonly LMSContext _context = context;

        public async Task<Exam> CreateAsync(Exam exam)
        {
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        public async Task<List<Exam>> GetAllAsync()
        {
            return await _context.Exams
                //.Include(e => e.Chapter) // Include the chapter details
                //.Include(e => e.QuizQuestions) // Include associated quiz questions
                //.Include(e => e.Answers) // Include associated answers
                .ToListAsync();
        }

        public async Task<Exam?> GetByIdAsync(int id)
        {
            return await _context.Exams
                //.Include(e => e.Chapter)
                //.Include(e => e.QuizQuestions)
                //.Include(e => e.Answers)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Exam?> UpdateAsync(int id, Exam updatedExam)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null) return null;

            exam.Title = updatedExam.Title;
            exam.TimeLimit = updatedExam.TimeLimit;
            exam.MaxQuestion = updatedExam.MaxQuestion;
            exam.Status = updatedExam.Status;
            exam.ChapterId = updatedExam.ChapterId;

            await _context.SaveChangesAsync();
            return exam;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null) return false;
            var quiz_question = await _context.QuizQuestions.Where(q => q.ExamId == id).ToListAsync();
            if(quiz_question != null)
            {
                _context.QuizQuestions.RemoveRange(quiz_question);
            }

            var answer = await _context.Answers.Where(a => a.ExamId == id).ToListAsync();
            if (answer != null) {
                _context.Answers.RemoveRange(answer);
            }
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
