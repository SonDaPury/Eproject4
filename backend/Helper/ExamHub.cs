using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace backend.Helper
{
    public class ExamHub : Hub
    {
        private readonly LMSContext _context;
        //private static Dictionary<int, (System.Timers.Timer Timer, DateTime EndTime)> _examTimers = new Dictionary<int, (System.Timers.Timer, DateTime)>();
        private readonly IExamService _examService;
        private readonly IHubContext<ExamHub> _hubContext;
        private static Dictionary<int, bool> _continueExams = new Dictionary<int, bool>();
        public ExamHub(LMSContext context, IExamService examService, IHubContext<ExamHub> hubContext)
        {
            _context = context;
            _examService = examService;
            _hubContext = hubContext;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = 1;
            //int.Parse(Context.User.Identity.Name);
            //var user = await _context.Users.FindAsync(userId);

            //if (user != null)
            //{
            //    var connection = new UserConnection
            //    {
            //        //ConnectionId = Context.ConnectionId,
            //        UserId = user.Id,
            //        ConnectedAt = DateTime.UtcNow
            //    };

            //    _context.UserConnections.Add(connection);
            //    await _context.SaveChangesAsync();
            //}
            var userConnection = await _context.UserConnections
            .OrderByDescending(uc => uc.ConnectedAt)
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ExamId != null);

            if (userConnection != null && DateTime.UtcNow < userConnection.EndTime)
            {
                var remainingTime = userConnection.EndTime - DateTime.UtcNow;
                if (userConnection.DisconnectedAt != null)
                {
                    var elapsed = (DateTime.UtcNow - userConnection.DisconnectedAt.Value).TotalSeconds;
                    await Clients.Caller.SendAsync("UpdateTime", remainingTime.TotalSeconds - elapsed);
                }
                else
                {
                    await Clients.Caller.SendAsync("UpdateTime", remainingTime.TotalSeconds);
                }

                if (!_continueExams.ContainsKey(userConnection.UserId))
                {
                    _continueExams[userConnection.UserId] = true;
                    _ = CountDown(userConnection.UserId, userConnection.EndTime);
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //var connection = await _context.UserConnections.FindAsync(Context.ConnectionId);
            //if (connection != null)
            //{
            //    connection.DisconnectedAt = DateTime.UtcNow;
            //    await _context.SaveChangesAsync();
            //}
            var userId = 1;
            //int.Parse(Context.User.Identity.Name); // Giả sử bạn có thể trích xuất ID người dùng từ context
            var userConnection = await _context.UserConnections
               .OrderByDescending(x => x.ConnectedAt)
               .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.DisconnectedAt == null);  // Giả định UserId = 1

            if (userConnection != null)
            {
                userConnection.DisconnectedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }


            await base.OnDisconnectedAsync(exception);
        }

        //public async Task SendTimeUpdate(string connectionId, string time)
        //{
        //    await Clients.Client(connectionId).SendAsync("ReceiveTimeUpdate", time);
        //}

        public async Task StartExam(int examId)
        {
            var userId = 1;
                //int.Parse(Context.User.Identity.Name); // Giả sử bạn có thể trích xuất ID người dùng từ context
            var exam = await _context.Exams.FindAsync(examId);

            if (exam == null || exam.IsStarted)
            {
                throw new Exception("Không tìm thấy kỳ thi hoặc kỳ thi đã bắt đầu.");
            }

            exam.IsStarted = true;
            await _context.SaveChangesAsync();

            //var userConnection = await _context.UserConnections.OrderByDescending(x => x.ConnectedAt)
            //                       .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.DisconnectedAt == null);

            //if (userConnection == null)
            //{
            //    throw new Exception("Không tìm thấy kết nối người dùng.");
            //}
            var now = DateTime.UtcNow;
            var endTime = now.AddMinutes(exam.TimeLimit);
            var userConnection = new UserConnection
            {
                UserId = userId,
                ConnectedAt = now,
                EndTime = endTime,
                ExamId = examId
            };

            _context.UserConnections.Add(userConnection);
            await _context.SaveChangesAsync();
            _ = CountDown(userId, endTime);
        }
        private async Task CountDown(int userId, DateTime endTime)
        {
            while (DateTime.UtcNow < endTime && _continueExams[userId])
            {
                var remainingTime = endTime - DateTime.UtcNow;
                var formattedTime = $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveTimeUpdate", formattedTime);
                await Task.Delay(1000); // Đợi một giây
            }

            if (_continueExams[userId])
            {
                // Giả sử bạn có cách để lấy examId từ userId, nếu không bạn cần lưu thêm thông tin
                var userConnection = await _context.UserConnections
                    .OrderByDescending(uc => uc.ConnectedAt)
                    .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ExamId != null);

                if (userConnection != null)
                {
                    await _examService.EndExam(userConnection.ExamId.Value, userId);
                    await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveExamEnd");
                }
            }
            _continueExams.Remove(userId); // Cleanup
        }

        public async Task EndExam( int examId)
        {
            var userId = 1;
            if (_continueExams.ContainsKey(userId))
            {
                _continueExams[userId] = false;
                _continueExams.Remove(userId);
            }

            await _examService.EndExam(examId, userId);
            //await Clients.Client(Context.ConnectionId).SendAsync("ReceiveExamEnd");
            await Clients.User(userId.ToString()).SendAsync("ReceiveExamEnd");
            //await _hubContext.Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveExamEnd");
        }
    }
}
