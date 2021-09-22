using iComplyICO.Data;
using Microsoft.EntityFrameworkCore;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Core.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickMeApp.Application.Repositories
{
    public class ChatRepository : IChatRepository
    {
        protected readonly ApplicationDbContext _dbContext;

        public ChatRepository(
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Chat>> GetChatsAsync(string userId, ResourceParameters resourceParameters)
        {
            var collection = _dbContext.Chats as IQueryable<Chat>;
            if (!string.IsNullOrEmpty(userId))
                collection = collection.Where(e => e.FirstUserId == userId || e.SecondUserId == userId);

            collection = collection.OrderByDescending(e => e.LastMessageTimeStamp);

            return await PagedList<Chat>.CreateAsync(collection,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }

        public async Task<PagedList<Message>> GetChatMessagesAsync(Guid chatId, ResourceParameters resourceParameters)
        {
            var collection = _dbContext.Messages
                .Where(e => e.ChatId == chatId)
                .OrderByDescending(e => e.Timestamp);

            return await PagedList<Message>.CreateAsync(collection,
                    resourceParameters.PageNumber,
                    resourceParameters.PageSize);
        }

        public async Task<Chat> CreateChat(string userId1, string userId2)
        {
            var chat = new Chat()
            {
                FirstUserId = userId1,
                SecondUserId = userId2
            };

            _dbContext.Chats.Add(chat);

            await _dbContext.SaveChangesAsync();

            return chat;
        }

        public async Task<Message> CreateMessageAsync(Guid chatId, string text, string senderId)
        {
            Message message = new Message()
            {
                ChatId = chatId,
                Text = text,
                SendUserId = senderId,
                Timestamp = DateTime.UtcNow
            };

            Chat chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
            if (chat == null)
                return null;

            chat.Messages = new List<Message>();

            chat.Messages.Add(message);
            chat.LastMessageTimeStamp = message.Timestamp;
            _dbContext.Chats.Update(chat);

            await _dbContext.SaveChangesAsync();

            return message;
        }

        public async Task<bool> ChatExistsAsync(string userId1, string userId2)
        {
            if (string.IsNullOrEmpty(userId1))
            {
                throw new ArgumentNullException(nameof(userId1));
            }
            if (string.IsNullOrEmpty(userId2))
            {
                throw new ArgumentNullException(nameof(userId2));
            }

            return await _dbContext.Chats.AnyAsync(e =>
                (e.FirstUserId == userId1 && e.SecondUserId == userId2) ||
                (e.FirstUserId == userId2 && e.SecondUserId == userId1));
        }
    }
}
