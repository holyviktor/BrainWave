using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BrainWave.WebUI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly BrainWaveDbContext _dbContext;
        public ChatHub(BrainWaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task SendMessage(string group, string userId, string message)
        {
            var userTag = (Context.GetHttpContext()?.User.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
            var user = _dbContext.Users.Include(u => u.Participants).ThenInclude(p => p.Conversation)
                .SingleOrDefault(u => u.Tag == userTag);

            int groupId = Int32.Parse(group);

            if (user == null)
            {
                throw new ArgumentException();
            }
            var conversation = _dbContext.Conversations.Find(groupId);
            if (conversation == null)
            {
                throw new ArgumentException();
            }
            var newMessage = new Message
            {
                Conversation= conversation,
                User= user,
                DateTimeCreated= DateTime.Now,
                Text = message
            };
            _dbContext.Messages.Add(newMessage);
            _dbContext.SaveChanges();

            await Clients.Groups(group).SendAsync("ReceiveMessage", userTag, message, user.Name, 
                user.Surname, user.Photo, newMessage.DateTimeCreated);
        }
    }
}
