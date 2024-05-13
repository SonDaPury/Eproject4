using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace backend.Helper
{
    public class ExamHub : Hub
    {
        private readonly LMSContext _context;
        public ExamHub(LMSContext context)
        {
            _context = context;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = 3;
                //int.Parse(Context.User.Identity.Name);
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                var connection = new UserConnection
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = user.Id,
                    ConnectedAt = DateTime.UtcNow
                };

                _context.UserConnections.Add(connection);
                await _context.SaveChangesAsync();
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connection = await _context.UserConnections.FindAsync(Context.ConnectionId);
            if (connection != null)
            {
                connection.DisconnectedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendTimeUpdate(string connectionId, string time)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveTimeUpdate", time);
        }

        public async Task EndExam(string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveExamEnd");
        }
    }
}
