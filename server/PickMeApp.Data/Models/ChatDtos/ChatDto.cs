using System;

namespace PickMeApp.Application.Models.ChatDtos
{
    public class ChatDto
    {
        public Guid ChatId { get; set; }

        public string FirstUserId { get; set; }

        public string FirstUserName { get; set; }

        public string FirstUserPhoto { get; set; }

        public string SecondUserId { get; set; }

        public string SecondUserName { get; set; }

        public string SecondUserPhoto { get; set; }

        public string LastMessageSenderId { get; set; }

        public int NumberOfUnreadedMessages { get; set; }
    }
}
