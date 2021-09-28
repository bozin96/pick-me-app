using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickMeApp.Web.Models
{
    public class UserChatViewModel
    {
            public string Id { get; set; }
            public Guid CurrentChat { get; set; }

        public UserChatViewModel()
        {

        }

        public UserChatViewModel(string userId)
        {
            Id = userId;
        }

        public UserChatViewModel(string userId, Guid chatId)
        {
            Id = userId;
            CurrentChat = chatId;
        }
    }
}
