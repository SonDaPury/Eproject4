using backend.Base;
using backend.Data;
using backend.Entities;
using backend.Exceptions;
using backend.Helper;
using backend.Service.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Timers;

namespace backend.Service
{
    public class ExamService(LMSContext context, IHubContext<ExamHub> hubContext, ISerialService serialService) : IExamService
    {
        private readonly LMSContext _context = context;
        private readonly IHubContext<ExamHub> _hubContext = hubContext;
        private readonly ISerialService _serialService = serialService;
        private static volatile bool _continueExam = true;

        public async Task<Exam> CreateAsync(Exam exam, int index)
        {
            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();
            var newSerial = new Serial
            {
                Index =index,
                ExamId = exam.Id,
                LessonId = null
            };
            await _serialService.CreateAsync(newSerial);
            await _context.SaveChangesAsync();
            return exam;
        }

        public async Task<List<Exam>> GetAllAsync()
        {
            return await _context.Exams.ToListAsync();
        }

        public async Task<(List<Exam>,int)> GetAllAsync(Pagination pagination)
        {
            var exams = await _context.Exams
                //.Include(e => e.Chapter) // Include the chapter details
                //.Include(e => e.QuizQuestions) // Include associated quiz questions
                //.Include(e => e.Answers) // Include associated answers
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
            var count = await _context.Exams.CountAsync(); 
            return (exams, count);
        }

        public async Task<dynamic> GetDetailsExam(int examId)
        {
            var exam = await _context.Exams
                .Where(e => e.Id == examId)
                .Select(e => new
                {
                    e.Id,
                    e.Title,
                    e.TimeLimit,
                    Questions = e.QuizQuestions.Select(qq => new
                    {
                        QuestionText = qq.Question.Content,
                        Options = qq.Question.Options.Select(o => new { o.Answer, o.IsCorrect })
                    })
                })
                .FirstOrDefaultAsync();

            return exam ?? throw new NotFoundException($"exam not found with id : {examId} ");
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
            exam.SourceId = updatedExam.SourceId;
            //exam.StaticFolder = updatedExam.StaticFolder;

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
            var serials = await _context.Serials.Where(q => q.ExamId == id).ToListAsync();
            if (serials != null)
            {
                _context.Serials.RemoveRange(serials);
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
            //System.Timers.Timer timer = new System.Timers.Timer(1000);
            //timer.Elapsed += async (sender, e) => {
            //    if (DateTime.UtcNow >= endTime || !_continueExam)
            //    {
            //        timer.Stop();
            //        exam.IsStarted = false;
            //        await _context.SaveChangesAsync();
            //        if (!_continueExam)
            //        {
            //            //await _hubContext.Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveExamEnd");
            //        }
            //        timer.Dispose();
            //    }
            //    else
            //    {
            //        var remainingTime = endTime - DateTime.UtcNow;
            //        var formattedTime = $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
            //        //await _hubContext.Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveTimeUpdate", formattedTime);
            //    }
            //};
            //timer.Start();
        }


        public async Task<int> EndExam(
            List<UserAnswer> userAnswers
            , int examId
            , int userId)
        {
            _continueExam = false;
            var exam = await _context.Exams.FindAsync(examId);
            var userConnection = await _context.UserConnections.OrderByDescending(x => x.ConnectedAt).FirstOrDefaultAsync(uc => uc.UserId == userId && uc.DisconnectedAt == null);
            if (exam == null || userConnection == null) throw new Exception("Exam or User not found");
            //await _hubContext.Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveExamEnd");
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
                var total = await CalculateScore(userAnswers, examId);
                var answer = new Answer
                {
                    Total = total.ToString() + "%",
                    UserId = userId,
                    ExamId = examId,
                    AttemptId = attemp.Id
                };
                await _context.Answers.AddAsync(answer);
                await _context.SaveChangesAsync();
                return total;
            }
            else
            {
                // Xử lý trường hợp DisconnectedAt hoặc ConnectedAt không có giá trị
                throw new Exception("DisconnectedAt or ConnectedAt not found");
            }

        }

        //tính điểm theo % nếu mỗi câu hỏi có 1 đáp án đúng
        private async Task<int> CalculateScore(List<UserAnswer> userAnswers, int examId)
        {

            // Lấy tất cả câu hỏi và câu trả lời đúng cho kỳ thi này
            var questions = await _context.QuizQuestions
                .Include(qq => qq.Question)
                    .ThenInclude(q => q.Options)
                .Where(qq => qq.ExamId == examId && qq.Question.Options.Any(o => o.IsCorrect))
                .ToListAsync();
            int totalQuestions = questions.Count;
            int correctAnswers = 0;
            // Kiểm tra từng câu trả lời của người dùng
            foreach (var userAnswer in userAnswers)
            {
                var correctOption = questions
                    .SelectMany(qq => qq.Question.Options)
                    .FirstOrDefault(o => o.QuestionId == userAnswer.QuestionId && o.IsCorrect);

                if (correctOption != null && correctOption.Id == userAnswer.OptionId)
                {
                    correctAnswers++; // Người dùng chọn đúng, tăng điểm
                }
            }

            return (int)correctAnswers / totalQuestions * 100;
        }

        //tính điểm nếu câu hỏi có nhiều đáp án đúng
        public async Task<int> CalculateScore1(List<UserAnswer> userAnswers, int examId)
        {
            var questions = await _context.QuizQuestions
                .Include(qq => qq.Question)
                    .ThenInclude(q => q.Options)
                .Where(qq => qq.ExamId == examId)
                .ToListAsync();

            int totalQuestions = questions.Count;
            int correctAnswers = 0;

            foreach (var question in questions)
            {
                var correctOptions = question.Question.Options.Where(o => o.IsCorrect).ToList();
                var userOptions = userAnswers.Where(ua => ua.QuestionId == question.QuestionId).Select(ua => ua.OptionId).ToList();

                if (correctOptions.All(co => userOptions.Contains(co.Id)) && userOptions.Count == correctOptions.Count)
                {
                    correctAnswers++;
                }
            }

            return (int)correctAnswers / totalQuestions * 100;
        }
    }

}
