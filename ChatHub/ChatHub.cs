using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRSingleWithMySql.Database;
using SignalRSingleWithMySql.Database.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalRSingleWithMySql.ChatHub
{
    public class ChatHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ChatContext _dbContext;

        public ChatHub(IHttpContextAccessor httpContextAccessor, ChatContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _dbContext = dbContext;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            var newMessage = new Message
            {
                SenderId = user,
                ReceiverId = "to all",  //for all
                Content = message,
                SentAt = DateTime.UtcNow // Optionally, you can set the timestamp
            };

            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();
        }


        public async Task SendToUser(string user, string receiverConnectionId, string message)
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", user, message);
            var newMessage = new Message
            {
                SenderId = user,
                ReceiverId = receiverConnectionId,
                Content = message,
                SentAt = DateTime.UtcNow // Optionally, you can set the timestamp
            };

            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();
        }

        public string GetConnectionId() => Context.ConnectionId;
    }
}
