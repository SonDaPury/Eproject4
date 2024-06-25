using backend.Base;
using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class AnswerService : IAnswerService
    {
        private readonly LMSContext _context;

        public AnswerService(LMSContext context)
        {
            _context = context;
        }

        public async Task<Answer> CreateAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<(List<Answer>, int)> GetAllAsync(Pagination pagination)
        {
            var answers = await _context.Answers
                //.Include(a => a.Exam)
                //.Include(a => a.User)
                //.Include(a => a.Attemp)
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
            var count = await _context.Answers.CountAsync();
            return (answers, count);
        }

        public async Task<Answer?> GetByIdAsync(int id)
        {
            return await _context.Answers
                //.Include(a => a.Exam)
                //.Include(a => a.User)
                //.Include(a => a.Attemp)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Answer?> UpdateAsync(int id, Answer updatedAnswer)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null) return null;

            answer.Total = updatedAnswer.Total;
            answer.ExamId = updatedAnswer.ExamId;
            answer.UserId = updatedAnswer.UserId;
            answer.AttemptId = updatedAnswer.AttemptId;

            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null) return false;

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Answer>> GetAllAsync()
        {
            return await _context.Answers.ToListAsync();
        }
        public async Task<object> GetAllByUserIdAndExamId(int userId, int examId)
        {

            var result = await (from answer in _context.Answers
                                join attemp in _context.Attemps
                                on answer.AttemptId equals attemp.Id
                                where answer.UserId == userId && answer.ExamId == examId
                                orderby attemp.Index
                                select new 
                                {
                                    AnswerId = answer.Id,
                                    Total = answer.Total,
                                    ExamId = answer.ExamId,
                                    UserId = answer.UserId,
                                    AttemptId = answer.AttemptId,
                                    Index = attemp.Index,
                                    TimeTaken = attemp.TimeTaken
                                }).ToListAsync();

            return result;

        }
    }

}
