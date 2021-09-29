using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickMeApp.Web.Models
{
    public class UserChatViewModel
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public Guid CurrentChat { get; set; }

        public UserChatViewModel()
        {

        }

        public UserChatViewModel(string userId)
        {
            UserId = userId;
        }

        public UserChatViewModel(string userId, string connectionId, Guid chatId)
        {
            UserId = userId;
            ConnectionId = connectionId;
            CurrentChat = chatId;
        }
    }

    public class ChatViewModel
    {
        public string ConnectionId { get; set; }
        public Guid CurrentChat { get; set; }

        public ChatViewModel()
        {

        }

        public ChatViewModel(string connectionId)
        {
            ConnectionId = connectionId;
        }

        public ChatViewModel(string connectionId, Guid chatId)
        {
            ConnectionId = connectionId;
            CurrentChat = chatId;
        }
    }
}
