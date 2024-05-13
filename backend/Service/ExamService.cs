using backend.Data;
using backend.Entities;
using backend.Helper;
using backend.Service.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class ExamService(LMSContext context, IHubContext<ExamHub> hubContext) : IExamService
    {
        private readonly LMSContext _context = context;
        private readonly IHubContext<ExamHub> _hubContext = hubContext;
        private static volatile bool _continueExam = true;

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
            if (quiz_question != null)
            {
                _context.QuizQuestions.RemoveRange(quiz_question);
            }

            var answer = await _context.Answers.Where(a => a.ExamId == id).ToListAsync();
            if (answer != null)
            {
                _context.Answers.RemoveRange(answer);
            }
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task StartExam(int examId, int userId)
        {
            var exam = await _context.Exams.FindAsync(examId);
            if (exam == null || exam.IsStarted)
            {
                throw new Exception("Exam not found or already started.");
            }
            exam.IsStarted = true;
            await _context.SaveChangesAsync();

            var userConnection = await _context.UserConnections.OrderByDescending(x => x.ConnectedAt).FirstOrDefaultAsync(uc => uc.UserId == userId && uc.DisconnectedAt == null);

            if (exam == null || userConnection == null) throw new Exception("Exam or User not found");

            var endTime = DateTime.UtcNow.AddMinutes(exam.TimeLimit);
            //var endTime = DateTime.UtcNow.AddMinutes(2);
            _continueExam = true;

            while (DateTime.UtcNow < endTime && _continueExam == true)
            { 
                var remainingTime = endTime - DateTime.UtcNow;
                var formattedTime = $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                await _hubContext.Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveTimeUpdate",formattedTime);
                await Task.Delay(1000);
            }
            exam.IsStarted = false;
            await _context.SaveChangesAsync();
            if (!_continueExam)
            {
                await _hubContext.Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveExamEnd");
            }
        }
        public async Task EndExam(int examId, int userId)
        {
            _continueExam = false;
            var exam = await _context.Exams.FindAsync(examId);
            var userConnection = await _context.UserConnections.OrderByDescending(x => x.ConnectedAt).FirstOrDefaultAsync(uc => uc.UserId == userId && uc.DisconnectedAt == null);
            if (exam == null || userConnection == null) throw new Exception("Exam or User not found");
            await _hubContext.Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveExamEnd");
            //await _hubContext.Clients.Client(userConnection.ConnectionId).SendAsync("DisconnectClient");
            userConnection.DisconnectedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            if (userConnection.DisconnectedAt.HasValue)
            {
                var timeTaken = userConnection.DisconnectedAt.Value - userConnection.ConnectedAt;
                var formattedTakenTime = $"{timeTaken.Minutes:D2}:{timeTaken.Seconds:D2}";
                // Tìm giá trị Index lớn nhất hiện tại
                int? maxIndex = await _context.Attemps.MaxAsync(a => (int?)a.Index) ?? 0;
                Attemp attemp = new Attemp()
                {
                    Index = maxIndex + 1,
                    TimeTaken = formattedTakenTime,
                    UserId = userId
                };
                exam.IsStarted = false;
                await _context.Attemps.AddAsync(attemp);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Xử lý trường hợp DisconnectedAt hoặc ConnectedAt không có giá trị
                throw new Exception("DisconnectedAt or ConnectedAt not found");
            }
            
        } 
    }

}
