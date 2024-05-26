using backend.Base;
using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly LMSContext _context;

        public QuestionService(LMSContext context)
        {
            _context = context;
        }

        public async Task<Question> CreateAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }
        public async Task<List<Question>> GetAllAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<(List<Question>,int)> GetAllAsync(Pagination pagination)
        {
            var questions = await _context.Questions
                 //.Include(q => q.QuizQuestions) // Include quiz questions associated with the question
                 //.Include(q => q.Options) // Include options for the question
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                 .Take(pagination.PageSize)
                .ToListAsync();
            var count = await _context.Questions.CountAsync();
            return (questions, count);  
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            return await _context.Questions
                //.Include(q => q.QuizQuestions)
                //.Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Question?> UpdateAsync(int id, Question updatedQuestion)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return null;

            question.Content = updatedQuestion.Content;
            question.Image = updatedQuestion.Image;
            question.StaticFolder = updatedQuestion.StaticFolder;
            // Ensure options and quiz questions are handled if necessary, might require additional logic

            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return false;
            var quiz_question = await _context.QuizQuestions.Where(q => q.QuestionId == id).ToListAsync();
            if (quiz_question != null)
            {
                _context.QuizQuestions.RemoveRange(quiz_question);
            }

            var options = await _context.Options.Where(a => a.QuestionId == id).ToListAsync();
            if (options != null)
            {
                _context.Options.RemoveRange(options);
            }
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
