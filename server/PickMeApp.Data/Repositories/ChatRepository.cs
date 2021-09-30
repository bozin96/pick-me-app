using iComplyICO.Data;
using Microsoft.EntityFrameworkCore;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Core.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (!string.IsNullOrEmpty(resourceParameters.SearchQuery))
            {
                string searchTerm = resourceParameters.SearchQuery.ToLower();
                collection = collection.Where(e => (e.FirstUserId == userId && (e.SecondUser.FirstName.ToLower().Contains(searchTerm) || e.SecondUser.LastName.ToLower().Contains(searchTerm))) ||
                                                   (e.SecondUserId == userId && (e.FirstUser.FirstName.ToLower().Contains(searchTerm) || e.FirstUser.LastName.ToLower().Contains(searchTerm))));
            }

            collection = collection.Include(e => e.FirstUser).Include(e => e.SecondUser);
            collection = collection.OrderByDescending(e => e.LastMessageTimeStamp);

            return await PagedList<Chat>.CreateAsync(collection,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }

        public async Task<Chat> GetChatByUsersAsync(string userId1, string userId2)
        {
            if (string.IsNullOrEmpty(userId1))
            {
                throw new ArgumentNullException(nameof(userId1));
            }
            if (string.IsNullOrEmpty(userId2))
            {
                throw new ArgumentNullException(nameof(userId2));
            }

            return await _dbContext.Chats.FirstOrDefaultAsync(e =>
                (e.FirstUserId == userId1 && e.SecondUserId == userId2) ||
                (e.FirstUserId == userId2 && e.SecondUserId == userId1));
        }

        public async Task<PagedList<Message>> GetChatMessagesAsync(Guid chatId, ResourceParameters resourceParameters)
        {
            var collection = _dbContext.Messages
                .Where(e => e.ChatId == chatId)
                .OrderByDescending(e => e.Timestamp);


            var count = await collection.CountAsync();
            var items = await collection
                .Skip((resourceParameters.PageNumber - 1) * resourceParameters.PageSize)
                .Take(resourceParameters.PageSize)
                .OrderBy(e=>e.Timestamp)
                .ToListAsync();
            return new PagedList<Message>(items, count, resourceParameters.PageNumber, resourceParameters.PageSize);
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

            chat = await _dbContext.Chats
                .Include(c => c.FirstUser)
                .Include(c => c.SecondUser)
                .FirstOrDefaultAsync(c => c.Id == chat.Id);
            return chat;
        }

        public async Task<Message> CreateMessageAsync(Guid chatId, string text, string senderId, bool isChatActive)
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

            chat.Messages = new List<Message>
            {
                message
            };
            chat.LastMessageTimeStamp = message.Timestamp;
            chat.LastMessageSenderId = senderId;
            if (!isChatActive)
                chat.NumberOfUnreadedMessages++;
            _dbContext.Chats.Update(chat);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
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

        public async Task CleanUnreadedMessagesCounterAsync(Guid chatId, string userId)
        {
            var chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
            if (chat == null)
                return;
            // If same user opens chat do nothing.
            if (chat.LastMessageSenderId == userId)
                return;

            chat.NumberOfUnreadedMessages = 0;
            _dbContext.Chats.Update(chat);
            await _dbContext.SaveChangesAsync();
        }
    }
}
