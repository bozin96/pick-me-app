﻿using PickMeApp.Application.Helpers;
using PickMeApp.Core.Models.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickMeApp.Application.Interfaces
{
    public interface IChatRepository
    {
        Task<PagedList<Chat>> GetChatsAsync(string userId, ResourceParameters resourceParameters);

        Task<PagedList<Message>> GetChatMessagesAsync(Guid chatId, ResourceParameters resourceParameters);

        Task<Chat> CreateChat(string userId1, string userId2);

        Task<Message> CreateMessageAsync(Guid chatId, string text, string senderId);

        Task<bool> ChatExistsAsync(string userId1, string userId2);
    }
}
