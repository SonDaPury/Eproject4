using AutoMapper;
using backend.Base;
using backend.Data;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly LMSContext _context;
        private readonly IMapper _mapper;

        public QuestionService(LMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<QuestionDto> CreateAsync(QuestionDto questionDto)
        {
            var question = _mapper.Map<Question>(questionDto);
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return questionDto;
        }

        public async Task<List<QuestionDto>> GetAllAsync()
        {
            var questions = await _context.Questions
                //.Include(q => q.QuizQuestions) // Include quiz questions associated with the question
                //.Include(q => q.Options) // Include options for the question
                .ToListAsync();
            return _mapper.Map<List<QuestionDto>>(questions);   
        }
        public async Task<(List<QuestionDto>,int)> GetAllAsync(Pagination pagination)
        {
            var questions = await _context.Questions
                //.Include(q => q.QuizQuestions) // Include quiz questions associated with the question
                //.Include(q => q.Options) // Include options for the question
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                 .Take(pagination.PageSize)
                .ToListAsync();
            var count = await _context.Questions.CountAsync();
            return (_mapper.Map<List<QuestionDto>>(questions),count);
        }
        public async Task<QuestionDto?> GetByIdAsync(int id)
        {
            var qt = await _context.Questions
                //.Include(q => q.QuizQuestions)
                //.Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
            return _mapper.Map<QuestionDto?>(qt);
        }

        public async Task<QuestionDto?> UpdateAsync(int id, QuestionDto updatedQuestion)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return null;

            question.Content = updatedQuestion.Content;
            question.Image = updatedQuestion.Image;
            //question.StaticFolder = updatedQuestion.StaticFolder;
            // Ensure options and quiz questions are handled if necessary, might require additional logic

            await _context.SaveChangesAsync();
            return _mapper.Map<QuestionDto>(question);
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
